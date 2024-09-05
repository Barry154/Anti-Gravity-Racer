// This script manages game logic, such as tracking lap times, updating the UI, sequencing SFX playback, etc.

// This script is based on code and techniques taught in the Cybernetic Walrus workshop hosted by UnityEDU and has been altered
// and includes multiple original additions for this project. Sections which were taken from the UnityEDU code are marked with
// 'start' and 'end' comments. All other code is my own.
// Workshop YouTube link: https://www.youtube.com/watch?v=ULDhOuU2JPY&list=PLX2vGYjWbI0SvPiKiMOcj_z9zCG7V9lkp&index=1
// GitHub repo link for code file (GameManager): https://github.com/Yeisonlop10/Hover-Racer/blob/master/Scripts/GameManager.cs

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Public static reference to itself. This is generally reffered to as a singleton and can be called from 
    // any other object in the scene without reference. However, 'singletons' are not always the best means to 
    // achieve this type of goal. Works fine for Unity, but must be mindful if using this type of object in 
    // other projects or environments
    public static GameManager instance;

    // Public enum which determines which game mode the Game Manager should track
    public enum GameMode { TimeAttack, PilotGauntlet };

    [Header("Game Settings")]
    [SerializeField] public GameMode gameMode;
    [SerializeField] public int maxLaps = 3;                // Number of laps to complete before game over
    [SerializeField] VehicleMechanics vehicleMechanics;     // Reference to the vehicle's movement and hover script
    public PIDController pidController;                     // Reference to the PID Controller

    [Header("UI Objects")]
    [SerializeField] public GameHUD gameHUD;                    // Reference to the game's HUD script
    [SerializeField] public GameObject gameHUDCanvas;           // Reference to the game's HUD canvas object
    [SerializeField] public GameObject gameOverScreen;          // Reference to the UI which should appear when the game is over
    [SerializeField] public GameObject gameFailScreen;          // Reference to the UI which should appear when the player fails the objective
    [SerializeField] public GameObject pauseScreen;             // Reference to the UI which should appear when the game is paused

    [Header("Game Fail UI Message")]
    [SerializeField] public TextMeshProUGUI fellOffTrack;       // Reference to the UI which should appear when the player falls off track
    [SerializeField] public TextMeshProUGUI vehicleDestroyed;   // Reference to the UI which should appear when the vehicle is destroyed

    [Header("Time Attack Game Over UI")]
    [SerializeField] public GameObject timeAttackAchievements;  // Game object which contains the text elements for the time attack achievements
    [SerializeField] public TextMeshProUGUI bestLapAchieved;    // TextMeshPro object which displays the best lap time when game is over
    [SerializeField] public TextMeshProUGUI highestSpeed;       // TextMeshPro object which displays the highest speed achieved when game is over

    [Header("Pilot's Gauntlet Game Over UI")]
    [SerializeField] public GameObject pilotGauntletAchievements;   // Game object which contains the text elements for the pilot's gauntlet achievements
    [SerializeField] public TextMeshProUGUI gauntletTotalTime;      // TextMeshPro object which displays the total time taken to complete the gaintlet when game is over
    [SerializeField] public TextMeshProUGUI totalTargetsDestroyed;  // TextMeshPro object which displays the total number of targets destroyed when game is over
    [SerializeField] public TextMeshProUGUI targetsDestroyedLap1;   // TextMeshPro object which displays the total number of targets destroyed in lap 1
    [SerializeField] public TextMeshProUGUI targetsDestroyedLap2;   // TextMeshPro object which displays the total number of targets destroyed in lap 2
    [SerializeField] public TextMeshProUGUI targetsDestroyedLap3;   // TextMeshPro object which displays the total number of targets destroyed in lap 3

    [Header("Pilot's Gauntlet Obstacle Spawner")]
    [SerializeField] SpawnObstacles spawnObstacles;         // Reference to the script which should spawn obstacles on track depending on lap number

    [Header("UI Animation")]
    [SerializeField] AnimationManager animationManager;     // Reference to the script which controls UI animation prompts
    [SerializeField] LevelLoader levelLoader;               // Reference to the script which controls scene transitions

    [Header("Audio Managers")]
    [SerializeField] MainThemeManager mainThemeManager;     // Reference to the script which controls game music playback
    [SerializeField] public SFXManager sfxManager;          // Reference to the script which controls SFX

    // Game management variables
    private float[] lapTimes;                       // An array which stores the player's lap times
    private float gauntletTime;                     // Floating point value which times the player's time to complete the gauntlet
    [HideInInspector] public int targetsDestroyed;  // Integer variable which counts how many targets have been destroyed
    private int[] targetsDestroyedPerLap;           // Interger list which stores the number of mines destroyed for each lap
    private int currentLap;                         // The current lap the player is on
    private bool gameOverTrigger = false;           // Boolean which determines if the game is over
    private bool startGame = false;                 // Boolean which determines if the game loop has started
    private float topSpeedReached;                  // Float which stores the highest speed achieved by the player
    public bool gameIsPaused;                      // Boolean which checks if the game is paused

    // Runs when the object is first created within the game, before the start method
    void Awake()
    {
        ////////////////////////////////////////////UnityEDU Code Start (GameManager)/////////////////////////////////////////
        // If instance has not been initialised, set instance to 'this' (this GameManager script)
        if (instance == null) 
        { 
            instance = this;
        }

        // If another GameManager already exists that is not this script, destroy this script (only one GameManager can be active at a time)
        else if (instance != this) { Destroy(gameObject); }
        ////////////////////////////////////////////UnityEDU Code End (GameManager)///////////////////////////////////////////

        // Begin playback of the in-game music (INTRO)
        mainThemeManager.PlayIntroMusic();
    }

    // Called every time an object is 'turned on'
    void OnEnable()
    {
        ////////////////////////////////////////////UnityEDU Code Start (GameManager)/////////////////////////////////////////
        // Coroutines are used to model behaviour over several frames (Unity Docs). Basically helps control the timing of when
        // certain events should occur
        StartCoroutine(Init());
        ////////////////////////////////////////////UnityEDU Code End (GameManager)///////////////////////////////////////////

        // Set the application framerate
        Application.targetFrameRate = 60;
    }

    // Used for yield returns, essentially like a 'pause', then do this
    IEnumerator Init()
    {
        ////////////////////////////////////////////UnityEDU Code Start (GameManager)/////////////////////////////////////////
        // Wait a single frame
        yield return new WaitForSeconds(0.1f);

        // Initialise the lap times array, targets destroyed per lap array, and start the game loop
        lapTimes = new float[maxLaps + 1];
        startGame = true;
        ////////////////////////////////////////////UnityEDU Code End (GameManager)///////////////////////////////////////////

        // Initialise the targets destroyed per lap array
        targetsDestroyedPerLap = new int[maxLaps + 1];
        
        // Begin playback of the in-game music (MAIN LOOP)
        mainThemeManager.PlayGameMusic();
    }

    private void Start()
    {
        // Check which game mode the Game Manager should run, then activate the relevant game over canvas TextMeshPro elements to display to the player
        // once they have completed the game
        if (gameMode == GameMode.TimeAttack)
        {
            timeAttackAchievements.SetActive(true);
        }

        else if (gameMode == GameMode.PilotGauntlet)
        {
            pilotGauntletAchievements.SetActive(true);
            targetsDestroyed = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ////////////////////////////////////////////UnityEDU Code Start (GameManager)/////////////////////////////////////////
        // Update vehicle speed display (UI)
        UpdateUI_Speed();
        ////////////////////////////////////////////UnityEDU Code End (GameManager)///////////////////////////////////////////

        // If the game has started, perform the follwing
        if (GameIsActive())
        {
            // Check if vehicle hull strength is 'critical', then play warning
            if (gameHUD.durabilityBar.value <= 200f && !animationManager.hullWarningPlayed)
            {
                // Play danger SFX
                sfxManager.PlayDangerAlertSFX();

                animationManager.StartHullWarningBlink();
                vehicleMechanics.smoke.Play(true);
                vehicleMechanics.damageSparks.Play(true);
            }

            // Game mode specific UI updates (Time Attack)
            if (gameMode == GameMode.TimeAttack)
            {
                // Calculate current lap time
                if (currentLap >= 1) { lapTimes[currentLap] += Time.deltaTime; }
                //lapTimes[currentLap] += Time.deltaTime;
                // Update the UI which displays the lap time
                UpdateUI_LapTime();
            }

            // Game mode specific UI updates (Pilot's Gauntlet)
            else if (gameMode == GameMode.PilotGauntlet)
            {
                // Calculate time till pilot gauntlet is complete
                if (currentLap >= 1) { gauntletTime += Time.deltaTime; }
                                
                // Update pilot gauntlet timer UI
                UpdateUI_GauntletTime();
                // Update the number of targets destroyed
                UpdateUI_TargetsDestroyed();
            }

            // Check if vehicle can boost depending on boost bar value
            VehicleBoostCheck();
            // Update boost bar UI
            UpdateUI_BoostBar(vehicleMechanics.isBoosting);

            // Check if the game should pause
            PauseGame();
        }

        // Check if game is over
        else if (gameOverTrigger)
        {
            // Check if the rigidbody component of the vehicle is not null
            if (vehicleMechanics.rb != null)
            {
                // Slow the vehicle down
                vehicleMechanics.rb.velocity *= 0.95f;
            }
        }
    }

    // Perform actions when the player completes a lap
    public void LapCompleted()
    {
        // Check if game is over, if so, exit function
        if (gameOverTrigger) return;

        // Play lap completed SFX
        sfxManager.PlayLapCompleteSFX();

        // Increment current lap
        currentLap += 1;

        // Update the UI which displays the current lap number
        UpdateUI_CurrentLapNumber();

        ///// Game mode specific UI updates (Time Attack)
        if (gameMode == GameMode.TimeAttack)
        {
            // Update the UI which displays the best lap time
            UpdateUI_BestLapTime();
        }

        // Game mode specific UI updates and game features (Pilot's Gauntlet)
        else if (gameMode == GameMode.PilotGauntlet)
        {
            // Spawn first set of mines for the first lap
            if (currentLap == 1)
            {
                spawnObstacles.SpawnMines();
                animationManager.StartMineWarning();
            }

            // Spawn the second set of mines, wall obstacles and play the first wall's animation on lap 2
            else if (currentLap == 2)
            {
                spawnObstacles.SpawnWalls();

                if (!animationManager.wallSpawnPlayed)
                {
                    animationManager.SpawnWallAnimation();
                }

                targetsDestroyedPerLap[currentLap - 1] = targetsDestroyed;
                Debug.Log(targetsDestroyedPerLap[currentLap - 1]);
            }

            // Spawn the final set of mines, pillar obstacles and play the first pillar's animation on lap 3
            else if (currentLap == 3)
            {
                spawnObstacles.SpawnPillars();

                if (!animationManager.pillarSpawnPlayed)
                {
                    animationManager.SpawnPillarAnimation();
                }

                targetsDestroyedPerLap[currentLap - 1] = Mathf.Max(0, targetsDestroyed - targetsDestroyedPerLap[currentLap - 2]);
                Debug.Log(targetsDestroyedPerLap[currentLap - 1]);
            }
        }

        // If the designated number of laps is completed, do the following
        if (currentLap > maxLaps)
        {
            targetsDestroyedPerLap[currentLap - 1] = Mathf.Max(0, targetsDestroyed - targetsDestroyedPerLap[currentLap - 2] - targetsDestroyedPerLap[currentLap - 3]);

            GameIsOver();
        }
    }

    ////////////////////////////////////////////UnityEDU Code Start (GameManager)/////////////////////////////////////////
    // Function which updates the lap times
    void UpdateUI_LapTime()
    {
        // Check if lap time UI exists, if so, update it
        if (gameHUD != null) { gameHUD.SetLapTime(lapTimes[currentLap]); }
    }

    // Determine if the game loop has begun
    public bool GameIsActive()
    {
        // Return the truth value of 'and' operation between booleans 'game started' and inverse of 'game over'
        // Should return true if 'startGame' is true and 'gameOverTrigger' is false
        return startGame && !gameOverTrigger;
    }

    // Function which updates the current lap number on the UI
    void UpdateUI_CurrentLapNumber()
    {
        if (gameHUD != null) { gameHUD.SetLapDisplay(currentLap, maxLaps); }
    }

    // Update the vehicle speed UI
    void UpdateUI_Speed()
    {
        if ((gameHUD != null) && vehicleMechanics != null) 
        {
            ////////////////////////////My Amendments To The UnityEDU Code Start///////////////////////////
            //Convert the current speed (m/s) to km/h
            float kmPerHour = 3.6f * vehicleMechanics.currentSpeed;
            // Take the absolute value of the above calculation
            float speedometer = Mathf.Abs(kmPerHour * 1.8f);

            // Tracks the top speed achieved by the player by checking if the speedometer value becomes greater than the highest speed achieved so far
            if (speedometer > topSpeedReached)
            {
                topSpeedReached = (int)(speedometer);
                // Set the game over text for the highest speed to the highest tracked speed
                highestSpeed.text = "Your highest speed: " + topSpeedReached.ToString() + " km/h";
            }
            ////////////////////////////My Amendments To The UnityEDU Code End/////////////////////////////

            // Update the speed UI element
            gameHUD.SetSpeedDisplay(speedometer); 
        }
    }
    ////////////////////////////////////////////UnityEDU Code End (GameManager)///////////////////////////////////////////

    // Updates the pilot's gauntlet timer
    void UpdateUI_GauntletTime()
    {
        gameHUD.SetGauntletTimer(gauntletTime);
    }

    // Updates the total amount of targets destroyed
    void UpdateUI_TargetsDestroyed()
    {
        gameHUD.SetTargetsDestroyed(targetsDestroyed, spawnObstacles.totalMineCount);
    }

    // Function which finds the best lap time and updates the UI
    void UpdateUI_BestLapTime()
    {
        if (gameHUD != null)
        {
            // If the player is on the first or second lap
            if (currentLap <= 2)
            {
                // Best lap can only be that of the first lap
                gameHUD.SetBestLap(lapTimes[1]);
            }

            // Otherwise, if the player has more than one recorded lap time
            else if (currentLap > 2)
            {
                // Set the best lap to the first time recorded
                float bestLapTime = lapTimes[1];

                // Iterate over the laptimes array, starting from the second element in the array
                for (int i = 1; i <= currentLap - 1; i++)
                {
                    // Determine if a laptime is shorter than that of the first best lap
                    if (lapTimes[i] < bestLapTime)
                    {
                        // Update the best lap time
                        bestLapTime = lapTimes[i];
                    }
                }

                // Update the HUD best lap time element
                gameHUD.SetBestLap(bestLapTime);
                // Update the game over best lap time element
                bestLapAchieved.text = "Best Lap: " + gameHUD.ConvertTimeToString(bestLapTime);
            }
        }
    }

    // Calls function in GameHUD to update the boost bar slider's value
    void UpdateUI_BoostBar(bool isBoosting)
    {
        gameHUD.SetBoostBar(isBoosting);
    }

    // Checks if the player can boost
    void VehicleBoostCheck()
    {
        // Allow the player to boost as long as there is boost to expend (slider has a value above 0)
        if (gameHUD.boostBar.value > 0)
        {
            vehicleMechanics.canBoost = true;
        }

        // Otherwise, do not allow the player to boost (there is no boost on the meter to be used)
        else
        {
            vehicleMechanics.canBoost = false;
        }
    }

    // Checks if the vehicle should receive damage
    public void CheckVehicleCollision(float damage)
    {
        // Update the durability slider UI element on the HUD
        gameHUD.SetDurabilityBar(damage);

        // If the vehicle durability reaches 0, the game objective has been failed and the vehicle should be destroyed
        if (gameHUD.durabilityBar.value <= 0)
        {
            // Call the specific game fail function
            VehicleIsDestroyed();
        }
    }

    // Do the following when the game is over
    void GameIsOver()
    {
        // Trigger game over
        gameOverTrigger = true;

        // Play game complete sfx
        sfxManager.PlayGameCompleteSFX();

        // Mode specific post game achievements (Time Attack)
        if (gameMode == GameMode.TimeAttack)
        {
            // Update the UI which displays the final lap times
            UpdateUI_BestLapTime();
        }

        // Mode specific post game achievements (Pilot's Gauntlet)
        else if (gameMode == GameMode.PilotGauntlet)
        {
            gauntletTotalTime.text = "Gauntlet Time: " + gameHUD.gauntletTime.text;
            totalTargetsDestroyed.text = "Total: " + gameHUD.targetsDestroyed.text;
            targetsDestroyedLap1.text = targetsDestroyedPerLap[1] + " / " + spawnObstacles.numMinesField1;
            targetsDestroyedLap2.text = targetsDestroyedPerLap[2] + " / " + spawnObstacles.numMinesField2;
            targetsDestroyedLap3.text = targetsDestroyedPerLap[3] + " / " + spawnObstacles.numMinesField3;
        }

        // Hide game HUD
        gameHUDCanvas.SetActive(false);
        // Display game over UI
        gameOverScreen.SetActive(true);
    }

    // Do the following when the player falls off track
    public void FellOffTrack()
    {
        // Trigger game over
        gameOverTrigger = true;

        // Play game failed SFX
        sfxManager.PlayGameFailedSFX();

        // Hide game HUD
        gameHUDCanvas.SetActive(false);
        // Activate specific fail message
        fellOffTrack.gameObject.SetActive(true);
        // Display game objective fail UI
        gameFailScreen.SetActive(true);
    }

    // Do the following when the vehicle is destroyed
    public void VehicleIsDestroyed()
    {
        // Trigger game over
        gameOverTrigger = true;

        // Play game failed SFX
        sfxManager.PlayGameFailedSFX();
        sfxManager.PlayExplosionSFX();

        // Trigger DestroyVehicle
        StartCoroutine(vehicleMechanics.DestroyVehicle(3f));

        // Hide game HUD
        gameHUDCanvas.SetActive(false);
        // Activate specific fail message
        vehicleDestroyed.gameObject.SetActive(true);
        // Display game objective fail UI
        gameFailScreen.SetActive(true);
    }

    // Pauses the game
    void PauseGame()
    {
        // Pause game condition
        if (Input.GetButtonDown("PauseButton") && !gameIsPaused)
        {
            gameIsPaused = true;
            pauseScreen.SetActive(true);
            // Set the timescale to 0 so that the gameplay 'freezes'
            Time.timeScale = 0f;
        }

        // Unpause game condition
        else if (Input.GetButtonDown("PauseButton") && gameIsPaused)
        {
            gameIsPaused = false;
            pauseScreen.SetActive(false);
            // Set the timescale back to 1 so that the gameplay 'unfreezes' and resumes
            Time.timeScale = 1.0f;
        }
    }

    // Resumes the game after pausing if the UI button element is pressed to resume the gameplay
    public void ResumeButton()
    {
        gameIsPaused = false;
        pauseScreen.SetActive(false);
        Time.timeScale = 1.0f;
    }

    // Restart the game by reloading the scene in which the game loop takes place
    public void Restart()
    {
        // Reset the PID controller (mainly to prevent derivative kick)
        pidController.Reset();

        // Unpause game
        gameIsPaused = false;
        pauseScreen.SetActive(false);
        Time.timeScale = 1.0f;

        // Reset canvas activations
        gameHUDCanvas.SetActive(true);
        gameOverScreen.SetActive(false);
        gameFailScreen.SetActive(false);

        // Reload the current scene
        levelLoader.StartSceneTransition(SceneManager.GetActiveScene().buildIndex);
    }

    // Return to main menu scene
    public void MainMenu()
    {
        // Unpause the game
        gameIsPaused = false;
        pauseScreen.SetActive(false);
        Time.timeScale = 1.0f;

        // Load the main menu scene by index
        levelLoader.StartSceneTransition(1);
    }
}
