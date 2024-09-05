// This script controls and sets all the HUD (heads up display) UI elements of the game to relay information to the player.

// This script is based on code and techniques taught in the Cybernetic Walrus workshop hosted by UnityEDU and has been altered
// and includes multiple original additions for this project. Sections which were taken from the UnityEDU code are marked with
// 'start' and 'end' comments. All other code is my own.
// Workshop YouTube link: https://www.youtube.com/watch?v=ULDhOuU2JPY&list=PLX2vGYjWbI0SvPiKiMOcj_z9zCG7V9lkp&index=1
// GitHub repo link for code file (LapTimeUI): https://github.com/Yeisonlop10/Hover-Racer/blob/master/Scripts/LapTimeUI.cs
// GitHub repo link for code file (ShipUI): https://github.com/Yeisonlop10/Hover-Racer/blob/master/Scripts/ShipUI.cs

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    [Header("General UI Elements:")]
    [SerializeField] public TextMeshProUGUI lapNumberText;
    [SerializeField] public TextMeshProUGUI vehicleSpeed;
    [SerializeField] public Slider boostBar;
    [SerializeField] public Slider durabilityBar;

    [Header("Time Attack UI Elements:")]
    [SerializeField] public GameObject timeAttackUI;
    [SerializeField] public TextMeshProUGUI currentLapTime;
    [SerializeField] public TextMeshProUGUI bestLapTime;

    [Header("Pilot's Gauntlet UI Elements:")]
    [SerializeField] public GameObject pilotGauntletUI;
    [SerializeField] public TextMeshProUGUI gauntletTime;
    [SerializeField] public TextMeshProUGUI targetsDestroyed;

    private void Awake()
    {
        // Set both game mode UI activations to false
        timeAttackUI.SetActive(false);
        pilotGauntletUI.SetActive(false);

        // Reset the text values of the general UI elements on awake
        lapNumberText.text = "";
        vehicleSpeed.text = "";
        boostBar.value = 0;
        durabilityBar.value = 1000;

        // Reset the text values of the time attack UI elements on awake
        currentLapTime.text = "";
        bestLapTime.text = "";

        // Reset the text values of the time attack UI elements on awake
        gauntletTime.text = "";
        targetsDestroyed.text = "";
    }

    private void Start()
    {
        if (GameManager.instance.gameMode == GameManager.GameMode.TimeAttack)
        {
            timeAttackUI.SetActive(true);
        }

        else if (GameManager.instance.gameMode == GameManager.GameMode.PilotGauntlet)
        {
            pilotGauntletUI.SetActive(true);
        }
    }

    ////////////////////////////////////////////UnityEDU Code Start (ShipUI)/////////////////////////////////////////
    // Update the lap number UI
    public void SetLapDisplay(int currentLap, int numLaps)
    {
        if (currentLap > numLaps) return;

        lapNumberText.text = currentLap.ToString() + "/" + numLaps.ToString();
    }
    ////////////////////////////////////////////UnityEDU Code End (ShipUI)///////////////////////////////////////////

    ////////////////////////////////////////////UnityEDU Code Start (LapTimeUI)/////////////////////////////////////////
    //////////////////////////////////Original code amended for context of this project/////////////////////////////////
    // Update the lap time UI
    public void SetLapTime(float lapTime)
    {
        // Set the text value of the TextMeshPro component
        currentLapTime.text = ConvertTimeToString(lapTime);
    }
    ////////////////////////////////////////////UnityEDU Code End (LapTimeUI)///////////////////////////////////////////

    ////////////////////////////////////////////UnityEDU Code Start (ShipUI)/////////////////////////////////////////
    // Update the speed display of the vehicle
    public void SetSpeedDisplay(float currentSpeed)
    {
        int speed = (int)(currentSpeed);
        vehicleSpeed.text = speed.ToString();
    }
    ////////////////////////////////////////////UnityEDU Code End (ShipUI)///////////////////////////////////////////

    // Update the best lap time UI
    public void SetBestLap(float lapTime)
    {
        // Set the text value of the TextMeshPro component
        bestLapTime.text = ConvertTimeToString(lapTime);
    }

    // Update the boost slider value
    public void SetBoostBar(bool isBoosting)
    {
        if (!isBoosting && boostBar.value <= 1)
        {
            boostBar.value += Time.deltaTime / 10;
        }

        else if (isBoosting)
        {
            boostBar.value -= Time.deltaTime / 2;
        }
    }

    // Update the durability bar value
    public void SetDurabilityBar(float damage)
    {
        durabilityBar.value -= damage;
    }

    // Update the pilot gauntlet timer
    public void SetGauntletTimer(float time)
    {
        gauntletTime.text = ConvertTimeToString(time);
    }

    // Update the number of targets destroyed
    public void SetTargetsDestroyed(int targets, int maxTargets)
    {
        targetsDestroyed.text = targets.ToString() + " / " + maxTargets;
    }

    ////////////////////////////////////////////UnityEDU Code Start (LapTimeUI)/////////////////////////////////////////
    // Convert Time.deltaTime float into a string formated as a timer
    public string ConvertTimeToString(float time)
    {
        // Convert the parsed time to values which represent minutes and seconds
        int minutes = (int)(time / 60);
        float seconds = time % 60f;

        // Convert the above variables into a text string of a time shown in minutes and seconds
        string timeString = minutes.ToString("00") + ":" + seconds.ToString("00.000");

        return timeString;
    }
    ////////////////////////////////////////////UnityEDU Code End (LapTimeUI)///////////////////////////////////////////
}
