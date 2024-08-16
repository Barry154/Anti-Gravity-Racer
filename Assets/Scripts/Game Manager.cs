// This script manages game logic, such as tracking lap times and updating the UI 

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
    [SerializeField] public GameObject gameFailScreen;          // Reference to the UI which should appear when the player falls off track
    [SerializeField] public GameObject pauseScreen;             // Reference to the UI which should appear when the game is paused
    [SerializeField] public TextMeshProUGUI bestLapAchieved;    // TextMeshPro object which displays the best lap time when game is over
    [SerializeField] public TextMeshProUGUI highestSpeed;       // TextMeshPro object which displays the highest speed achieved when game is over

    // Game management variables
    private float[] lapTimes;               // An array which stores the player's lap times
    private int currentLap;                 // The current lap the player is on
    private bool gameOverTrigger = false;   // Boolean which determines if the game is over
    private bool startGame = false;         // Boolean which determines if the game loop has started
    private float topSpeedReached;          // Float which stores the highest speed achieved by the player
    private bool gameIsPaused;              // Boolean which checks if the game is paused

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
    }

    // Used for yield returns, essentially like a 'pause', then do this
    IEnumerator Init()
    {
        // Wait a single frame
        yield return null;

        // Initialise the lap times array and start the game loop
        lapTimes = new float[maxLaps + 1];
        startGame = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Update vehicle speed display (UI)
        UpdateUI_Speed();

        // If the game has started, perform the follwing
        if (GameIsActive())
        {
            // Calculate current lap time
            lapTimes[currentLap] += Time.deltaTime;
            // Update the UI which displays the lap time
            UpdateUI_LapTime();

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
        // Update the UI which displays the best lap time
        UpdateUI_BestLapTime();
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // If the designated number of laps is completed, do the following
        if (currentLap > maxLaps)
        {
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
            GameIsOver();
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }

        //Debug.Log("LapCompleted called");
    }

    // Function which updates the lap times
    void UpdateUI_LapTime()
    {
        // Check if lap time UI exists, if so, update it
        if (gameHUD != null) { gameHUD.SetLapTime(lapTimes[currentLap]); }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Function which finds the best lap time and updates the UI
    void UpdateUI_BestLapTime()
    {
        if(gameHUD != null)
        {
            if (currentLap == 2)
            {
                gameHUD.SetBestLap(lapTimes[1]);
            }

            else if (currentLap > 2) 
            {
                float bestLapTime = lapTimes[1];

                for (int i = 1; i <= currentLap -1; i++)
                {
                    //Debug.Log("Lap time " + i + ": " + lapTimes[i]);

                    if (lapTimes[i] < bestLapTime)
                    {
                        bestLapTime = lapTimes[i];
                    }
                }

                gameHUD.SetBestLap(bestLapTime);
                bestLapAchieved.text = "Best Lap: " + gameHUD.ConvertTimeToString(bestLapTime);
                //Debug.Log("Current best lap: " + bestLapTime);
            }
        }
    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

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
            float speedometer = Mathf.Abs(kmPerHour * 1.5f);

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
    void UpdateUI_BoostBar(bool isBoosting)
    {
        gameHUD.SetBoostBar(isBoosting);
    }

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
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

    // Determine if the game loop has begun
    public bool GameIsActive()
    {
        // Return the truth value of 'and' operation between booleans 'game started' and inverse of 'game over'
        // Should return true if 'startGame' is true and 'gameOverTrigger' is false
        return startGame && !gameOverTrigger;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
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

    public void ResumeButton()
    {
        gameIsPaused = false;
        pauseScreen.SetActive(false);
        Time.timeScale = 1.0f;
    }

    // Do the following when the game is over
    void GameIsOver()
    {
        // Trigger game over
        gameOverTrigger = true;
        // Update the UI which displays the final lap times
        UpdateUI_BestLapTime();
        // Hide game HUD
        gameHUDCanvas.SetActive(false);
        // Display game over UI
        gameOverScreen.SetActive(true);
    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Do the following when the player falls off track
    public void FellOffTrack()
    {
        // Trigger game over
        gameOverTrigger = true;
        // Hide game HUD
        gameHUDCanvas.SetActive(false);
        // Display game objective fail UI
        gameFailScreen.SetActive(true);
    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

    // Restart the game by reloading the scene in which the game loop takes place
    public void Restart()
    {
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        pidController.Reset();
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        gameIsPaused = false;
        pauseScreen.SetActive(false);
        Time.timeScale = 1.0f;

        gameHUDCanvas.SetActive(true);
        gameOverScreen.SetActive(false);
        gameFailScreen.SetActive(false);
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void MainMenu()
    {
        gameIsPaused = false;
        pauseScreen.SetActive(false);
        Time.timeScale = 1.0f;

        SceneManager.LoadScene("Main Menu");
    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
