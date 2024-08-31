using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VehicleColliderCheck : MonoBehaviour
{
    [Header("Vehicle Mechanics:")]
    [SerializeField] VehicleMechanics vehicleMechanics;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Pillar"))
        {
            // Play collision sound
            GameManager.instance.sfxManager.PlayCollisionSFX();

            // Calculate how much damage the vehicle should receive based on current speed
            float collisionForce = (Mathf.Abs(vehicleMechanics.currentSpeed) / 5);
            GameManager.instance.CheckVehicleCollision(collisionForce);
        }

        if (collision.gameObject.CompareTag("Mine"))
        {
            // Play collision sound
            GameManager.instance.sfxManager.PlayCollisionSFX();

            // Deal damage to vehicle
            GameManager.instance.CheckVehicleCollision(200);
        }
    }

    private void OnCollisionStay(Collision collision) 
    {
        vehicleMechanics.wallGrind.transform.position = collision.contacts[0].point;

        vehicleMechanics.frictionScale = 0.2f;

        if ((vehicleMechanics.currentSpeed > 1.5f || vehicleMechanics.currentSpeed < -1.5f) && GameManager.instance.GameIsActive())
        {
            vehicleMechanics.wallGrind.Play(true);
            GameManager.instance.CheckVehicleCollision(0.1f);

            // Play wallgrind SFX
            GameManager.instance.sfxManager.PlayWallgrindSFX();
        }
        
        else
        {
            vehicleMechanics.wallGrind.Stop(true);

            // Stop wallgrind SFX
            GameManager.instance.sfxManager.StopWallgrindSFX();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        vehicleMechanics.frictionScale = 0.4f;
        vehicleMechanics.wallGrind.Stop(true);

        // Stop wallgrind SFX
        GameManager.instance.sfxManager.StopWallgrindSFX();
    }
}
