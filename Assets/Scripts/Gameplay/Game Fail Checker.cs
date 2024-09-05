// This script check if the player has failed the game by falling off the track

using UnityEngine;

public class GameFailChecker : MonoBehaviour
{
    // Called when objects enter the trigger collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entered matches either of the vehicle collider tags
        if (other.CompareTag("vehicleNoseCollider") || other.CompareTag("vehicleWingCollider"))
        {
            // End the game using the specific 'game fail' conditions from the Game Manager
            GameManager.instance.FellOffTrack();
        }
    }
}
