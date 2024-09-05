// This script controls the the collision interactions of vehicle

// This script uses a technique learnt from the Cybernetic Walrus workshop hosted by UnityEDU and is marked with
// comments. All other code is my own.
// Workshop YouTube link: https://www.youtube.com/watch?v=ULDhOuU2JPY&list=PLX2vGYjWbI0SvPiKiMOcj_z9zCG7V9lkp&index=1
// GitHub repo link for code file (VehicleAVFX): https://github.com/Yeisonlop10/Hover-Racer/blob/master/Scripts/VehicleAVFX.cs

using UnityEngine;

public class VehicleColliderCheck : MonoBehaviour
{
    [Header("Vehicle Mechanics:")]
    [SerializeField] VehicleMechanics vehicleMechanics;

    // When collisions occur with the vehicle
    private void OnCollisionEnter(Collision collision)
    {
        // If the game object colliding with the vehicle are the track boundaries or obstacles
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Pillar"))
        {
            // Play collision sound
            GameManager.instance.sfxManager.PlayCollisionSFX();

            // Calculate how much damage the vehicle should receive based on current speed
            float collisionForce = (Mathf.Abs(vehicleMechanics.currentSpeed) / 5);
            // Deal damage to vehicle
            GameManager.instance.CheckVehicleCollision(collisionForce);
        }

        // If the game object colliding with the vehicle is a mine
        if (collision.gameObject.CompareTag("Mine"))
        {
            // Play collision sound
            GameManager.instance.sfxManager.PlayCollisionSFX();

            // Deal damage to vehicle
            GameManager.instance.CheckVehicleCollision(200);
        }
    }

    // If the collision persists
    private void OnCollisionStay(Collision collision) 
    {
        // Set the position of the 'wallgrind' particle effect to the first contact point between the two objects
        vehicleMechanics.wallGrind.transform.position = collision.contacts[0].point; // Sourced from UnityEDU VehicleAVFX

        // If the vehicle is above a certain speed (forward or reverse) and the game is not over
        if ((vehicleMechanics.currentSpeed > 1.5f || vehicleMechanics.currentSpeed < -1.5f) && GameManager.instance.GameIsActive())
        {
            // Play the wallgrind particle effect
            vehicleMechanics.wallGrind.Play(true);
            // Deal small amounts of damage to the vehicle
            GameManager.instance.CheckVehicleCollision(0.1f);

            // Play wallgrind SFX
            GameManager.instance.sfxManager.PlayWallgrindSFX();
        }
        
        else
        {
            // Stop the wallgrind particle effect
            vehicleMechanics.wallGrind.Stop(true);

            // Stop wallgrind SFX
            GameManager.instance.sfxManager.StopWallgrindSFX();
        }
    }

    // When the persistant collision ends
    private void OnCollisionExit(Collision collision)
    {
        // Stop the wallgrind particle effect
        vehicleMechanics.wallGrind.Stop(true);

        // Stop wallgrind SFX
        GameManager.instance.sfxManager.StopWallgrindSFX();
    }
}
