// This script manages game logic, such as tracking lap times and updating the UI 

using System;
using System.Collections;
using System.Collections.Generic;
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

    [Header("UI Animation")]
    [SerializeField] AnimationManager animationManager;     // Reference to the script which controls UI animation prompts
    [SerializeField] LevelLoader levelLoader;               // Reference to the script which controls scene transitions

    [Header("Pilot's Gauntlet Obstacle Spawner")]
    [SerializeField] SpawnObstacles spawnObstacles;         // Reference to the script which should spawn obstacles on track depending on lap number

    // Game management variables
    private float[] lapTimes;                       // An array which stores the player's lap times
    private float gauntletTime;                     // Floating point value which times the player's time to complete the gauntlet
    [HideInInspector] public int targetsDestroyed;  // Integer variable which counts how many targets have been destroyed
    private int[] targetsDestroyedPerLap;           // Interger list which stores the number of mines destroyed for each lap
    private int currentLap;                         // The current lap the player is on
    private bool gameOverTrigger = false;           // Boolean which determines if the game is over
    private bool startGame = false;                 // Boolean which determines if the game loop has started
    private float topSpeedReached;                  // Float which stores the highest speed achieved by the player
    private bool gameIsPaused;                      // Boolean which checks if the game is paused

    // Runs when the object is first created within the game, before the start method
    void Awake()
    {
        // If instance has not been initialised, set instance to 'this' (this GameManager script)
        if (instance == null) 
        { 
            instance = this;
        }

        // If another GameManager already exists that is not this script, destroy this script (only one GameManager can be active at a time)
        else if (instance != this) { Destroy(gameObject); }
    }

    // Called every time an object is 'turned on'
    void OnEnable()
    {
        // Coroutines are used to model behaviour over several frames (Unity Docs). Basically helps control the timing of when
        // certain events should occur
        StartCoroutine(Init());

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        Application.targetFrameRate = 60;
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }

    // Used for yield returns, essentially like a 'pause', then do this
    IEnumerator Init()
    {
        // Wait a single frame
        yield return null;

        // Initialise the lap times array, targets destroyed per lap array, and start the game loop
        lapTimes = new float[maxLaps + 1];
        targetsDestroyedPerLap = new int[maxLaps + 1];
        startGame = true;
    }

    private void Start()
    {
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
        // Update vehicle speed display (UI)
        UpdateUI_Speed();

        // If the game has started, perform the follwing
        if (GameIsActive())
        {
            // Check if vehicle hull strength is 'critical', then play warning
            if (gameHUD.durabilityBar.value <= 900f && !animationManager.hullWarningPlayed)
            {
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

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Check if vehicle can boost depending on boost bar value
            VehicleBoostCheck();
            // Update boost bar UI
            UpdateUI_BoostBar(vehicleMechanics.isBoosting);

            // Check if the game should pause
            PauseGame();
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }

        //Debug.Log(currentLap);
    }

    // Perform actions when the player completes a lap
    public void LapCompleted()
    {
        // Check if game is over, if so, exit function
        if (gameOverTrigger) return;

        // Increment current lap
        currentLap += 1;

        // Update the UI which displays the current lap number
        UpdateUI_CurrentLapNumber();

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
            Debug.Log(targetsDestroyedPerLap[currentLap - 1]);

            GameIsOver();
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }

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
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
            float kmPerHour = 3.6f * vehicleMechanics.currentSpeed;
            //Debug.Log(kmPerHour);
            float speedometer = Mathf.Abs(kmPerHour * 1.8f);

            if (speedometer > topSpeedReached)
            {
                topSpeedReached = (int)(speedometer);
                highestSpeed.text = "Your highest speed: " + topSpeedReached.ToString() + " km/h";
            }
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

            gameHUD.SetSpeedDisplay(speedometer); 
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
            if (currentLap <= 2)
            {
                gameHUD.SetBestLap(lapTimes[1]);
                //bestLapAchieved.text = "Best Lap: " + gameHUD.ConvertTimeToString(lapTimes[1]);
            }

            else if (currentLap > 2)
            {
                float bestLapTime = lapTimes[1];

                for (int i = 1; i <= currentLap - 1; i++)
                {
                    //Debug.Log("Lap time " + i + ": " + lapTimes[i]);

                    if (lapTimes[i] < bestLapTime)
                    {
                        bestLapTime = lapTimes[i];
                    }
                }

                gameHUD.SetBestLap(bestLapTime);
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
        if (gameHUD.boostBar.value > 0)
        {
            vehicleMechanics.canBoost = true;
        }

        else
        {
            vehicleMechanics.canBoost = false;
        }
    }

    // Checks if the vehicle should receive damage
    public void CheckVehicleCollision(float damage)
    {
        gameHUD.SetDurabilityBar(damage);

        if (gameHUD.durabilityBar.value <= 0)
        {
            VehicleIsDestroyed();
        }
    }

    // Do the following when the game is over
    void GameIsOver()
    {
        // Trigger game over
        gameOverTrigger = true;

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
        if (Input.GetButtonDown("PauseButton") && !gameIsPaused)
        {
            gameIsPaused = true;
            pauseScreen.SetActive(true);
            Time.timeScale = 0f;
        }

        else if (Input.GetButtonDown("PauseButton") && gameIsPaused)
        {
            gameIsPaused = false;
            pauseScreen.SetActive(false);
            Time.timeScale = 1.0f;
        }
    }

    // Resumes the game after pausing
    public void ResumeButton()
    {
        gameIsPaused = false;
        pauseScreen.SetActive(false);
        Time.timeScale = 1.0f;
    }

    // Restart the game by reloading the scene in which the game loop takes place
    public void Restart()
    {
        pidController.Reset();

        gameIsPaused = false;
        pauseScreen.SetActive(false);
        Time.timeScale = 1.0f;

        gameHUDCanvas.SetActive(true);
        gameOverScreen.SetActive(false);
        gameFailScreen.SetActive(false);

        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        levelLoader.StartSceneTransition(SceneManager.GetActiveScene().buildIndex);
    }

    // Return to main menu scene
    public void MainMenu()
    {
        gameIsPaused = false;
        pauseScreen.SetActive(false);
        Time.timeScale = 1.0f;

        //SceneManager.LoadScene("Main Menu");
        levelLoader.StartSceneTransition(1);
    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
