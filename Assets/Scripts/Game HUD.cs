using System.Collections;
using System.Collections.Generic;
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

    // Update the lap number UI
    public void SetLapDisplay(int currentLap, int numLaps)
    {
        if (currentLap > numLaps) return;
        //Debug.Log("SetLapDisplayCalled");
        //Debug.Log(currentLap);

        lapNumberText.text = currentLap.ToString() + "/" + numLaps.ToString();
    }

    // Update the lap time UI
    public void SetLapTime(float lapTime)
    {
        // Set the text value of the TextMeshPro component
        currentLapTime.text = ConvertTimeToString(lapTime);
    }

    // Update the speed display of the vehicle
    public void SetSpeedDisplay(float currentSpeed)
    {
        int speed = (int)(currentSpeed);
        vehicleSpeed.text = speed.ToString();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
    public void SetTargetsDestroyed(int targets)
    {
        targetsDestroyed.text = targets.ToString() + " / 3";
    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

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
}
