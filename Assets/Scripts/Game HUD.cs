using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameHUD : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI lapNumberText;
    [SerializeField] public TextMeshProUGUI currentLapTime;
    [SerializeField] public TextMeshProUGUI bestLapTime;
    [SerializeField] public TextMeshProUGUI vehicleSpeed;

    private void Awake()
    {
        // Reset the text values of the UI elements on awake
        lapNumberText.text = "0";
        currentLapTime.text = "";
        bestLapTime.text = "";
        vehicleSpeed.text = "";
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

    // Update the best lap time UI
    public void SetBestLap(float lapTime)
    {
        // Set the text value of the TextMeshPro component
        bestLapTime.text = ConvertTimeToString(lapTime);
    }

    public void SetSpeedDisplay(float currentSpeed)
    {
        int speed = (int)(currentSpeed);
        vehicleSpeed.text = speed.ToString();
    }

    string ConvertTimeToString(float time)
    {
        // Convert the parsed time to values which represent minutes and seconds
        int minutes = (int)(time / 60);
        float seconds = time % 60f;

        // Convert the above variables into a text string of a time shown in minutes and seconds
        string timeString = minutes.ToString("00") + ":" + seconds.ToString("00.000");

        return timeString;
    }
}
