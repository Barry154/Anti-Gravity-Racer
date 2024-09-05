// This script communicates with the 'Game Manager' to call the function which performs lap completed events. It works in conjection
// with the 'Lap Checkpoint' script to determine if a lap can be completed (prevents player cheating)

// This script is based on code from the Cybernetic Walrus workshop hosted by UnityEDU. Sections which were taken from the UnityEDU
// code are marked with 'start' and 'end' comments. One addition has been made which is original and has been duelly commented.
// Workshop YouTube link: https://www.youtube.com/watch?v=ULDhOuU2JPY&list=PLX2vGYjWbI0SvPiKiMOcj_z9zCG7V9lkp&index=1
// GitHub repo link for code file (FinishLine): https://github.com/Yeisonlop10/Hover-Racer/blob/master/Scripts/FinishLine.cs

using UnityEngine;

public class FinishLine : MonoBehaviour
{
    ////////////////////////////////////////////UnityEDU Code Start (FinishLine)/////////////////////////////////////////
    [HideInInspector] public bool canLap;

    [SerializeField] public bool debugMode;

    //////My Own Code Start//////
    private void Start()
    {
        canLap = true;
    }
    ///////My Own Code End///////


    // Called when objects enter the trigger collider
    private void OnTriggerEnter(Collider other)
    {
        // If the following conditions equate to true (checkpoint has been passed and the object matches the vehicle collider tag)
        if ((canLap || debugMode) && other.gameObject.CompareTag("vehicleNoseCollider"))
        {
            // Call the LapCompleted function in the Game Manager
            GameManager.instance.LapCompleted();
            // Reset the boolean which determines if the checkpoint has been passed
            canLap = false;
        }
    }
    ////////////////////////////////////////////UnityEDU Code End (FinishLine)///////////////////////////////////////////
}
