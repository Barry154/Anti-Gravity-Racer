// This script communicates with the 'Finish Line' script to let it kow that a lap can be completed (player has passed the mandatory lap checkpoint)

// This script is based on code from the Cybernetic Walrus workshop hosted by UnityEDU. Sections which were taken from the UnityEDU
// code are marked with 'start' and 'end' comments.
// Workshop YouTube link: https://www.youtube.com/watch?v=ULDhOuU2JPY&list=PLX2vGYjWbI0SvPiKiMOcj_z9zCG7V9lkp&index=1
// GitHub repo link for code file (LapChecker): https://github.com/Yeisonlop10/Hover-Racer/blob/master/Scripts/LapChecker.cs

using UnityEngine;

public class LapCheckpoint : MonoBehaviour
{
    ////////////////////////////////////////////UnityEDU Code Start (LapChecker)/////////////////////////////////////////
    [SerializeField] public FinishLine finishLine;

    // Called when objects enter the trigger collider
    private void OnTriggerEnter(Collider other)
    {
        // If the object entering the trigger matches the vehicle collider tag
        if (other.gameObject.CompareTag("vehicleNoseCollider")) 
        { 
            // Set the 'checkpoint passed' boolean to true
            finishLine.canLap = true;
        }
    }
    ////////////////////////////////////////////UnityEDU Code End (LapChecker)///////////////////////////////////////////
}
