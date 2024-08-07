// This script manages game logic, such as tracking lap times and updating the UI 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Public static reference to itself. This is generally reffered to as a singleton and can be called from 
    // any other object in the scene without reference. However, 'singletons' are not always the best means to 
    // achieve this type of goal. Works fine for Unity, but must be mindful if using this type of object in 
    // other projects or environments
    public static GameManager instance;

    [Header("Game Settings")]
    [SerializeField] public int maxLaps = 3;                // Number of laps to complete before game over
    [SerializeField] VehicleMechanics vehicleMechanics;     // Reference to the vehicle's movement and hover script
    [SerializeField] PIDController pidController;           // Reference to the PID Controller

    [Header("UI Objects")]
    //[SerializeField] public LapTimeUI lapTimeUI;
    //[SerializeField] public VehicleDataUI vehicleDataUI;
    [SerializeField] public GameObject gameOverScreen;

    // Game management variables
    private float[] lapTimes;               // An array which stores the player's lap times
    private int currentLap;                 // The current lap the player is on
    private bool gameOverTrigger = false;   // Boolean which determines if the game is over
    private bool startGame = false;         // Boolean which determines if the game loop has started

    // Runs when the object is first created within the game, before the start method
    private void Awake()
    {
        // If instance has not been initialised, set instance to 'this' (this GameManager script)
        if (instance == null) 
        { 
            instance = this;

            // Reset the derivative initialised value in the PIDContoller class
            //pidController.Reset();
        }

        // If another GameManager already exists that is not this script, destroy this script (only one GameManager can be active at a time)
        else if (instance != this) { Destroy(gameObject); }
    }

    // Called every time an object is 'turned on'
    private void OnEnable()
    {
        // Coroutines are used to model behaviour over several frames (Unity Docs). Basically helps control the timing of when
        // certain events should occur
        StartCoroutine(Init());
    }

    // Used for yield returns, essentially like a 'pause', then do this
    IEnumerator Init()
    {
        // Update the UI which displays the current lap
        // UpdateUI_LapNumber();

        // Wait a single frame
        yield return null;

        // Initialise the lap times array and start the game loop
        lapTimes = new float[maxLaps];
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
        }
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

        // If the designated number of laps is completed, do the following
        if (currentLap >= maxLaps)
        {
            // Trigger game over
            gameOverTrigger = true;
            // Update the UI which displays the final lap times
            UpdateUI_FinalLapTimes();
            // Display game over UI
            gameOverScreen.SetActive(true);
        }
    }

    // Function which updates the lap times
    void UpdateUI_LapTime()
    {
        // Check if lap time UI exists, if so, update it
        //if (lapTimeUI != null) { lapTimeUI.SetLapTime(currentLap, lapTimes[currentLap]); }
    }

    // Function which performs the final update on lap times
    void UpdateUI_FinalLapTimes()
    {
        //if (lapTimeUI != null)
        //{
        //    float total = 0f;

        //    // Calculate the accumilated time taken to complete the designated number of laps for the game
        //    for (int i = 0; i < lapTimes.Length; i++)
        //    {
        //        total += lapTimes[i];
        //    }

        //    lapTimeUI.SetFinalTime(total);
        //}
    }

    // Function which updates the current lap number on the UI
    void UpdateUI_CurrentLapNumber()
    {
        //if (lapTimeUI != null) { lapTimeUI.SetLapDisplay(currentLap + 1, maxLaps); }
    }

    // Update the vehicle speed UI
    void UpdateUI_Speed()
    {
        //if ((vehicleDataUI != null) && vehicleMechanics != null) { vehicleDataUI.SetSpeedDisplay(Mathf.Abs(vehicleMechanics.currentSpeed)); }
    }

    // Determine if the game loop has begun
    public bool GameIsActive()
    {
        // Return the truth value of 'and' operation between booleans 'game started' and inverse of 'game over'
        // Should return true if 'startGame' is true and 'gameOverTrigger' is false
        return startGame && !gameOverTrigger;
    }

    // Restart the game by reloading the scene in which the game loop takes place
    public void Restart()
    {
        pidController.Reset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
