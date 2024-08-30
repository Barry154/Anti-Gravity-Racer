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
            float collisionForce = (Mathf.Abs(vehicleMechanics.currentSpeed) / 5);
            GameManager.instance.CheckVehicleCollision(collisionForce);
        }

        if (collision.gameObject.CompareTag("Mine"))
        {
            GameManager.instance.CheckVehicleCollision(200);
        }
    }

    private void OnCollisionStay(Collision collision) 
    {
        vehicleMechanics.wallGrind.transform.position = collision.contacts[0].point;

        vehicleMechanics.frictionScale = 0.2f;

        if (vehicleMechanics.currentSpeed > 1.5f || vehicleMechanics.currentSpeed < -1.5f)
        {
            vehicleMechanics.wallGrind.Play(true);
            GameManager.instance.CheckVehicleCollision(0.1f);
        }
        
        else
        {
            vehicleMechanics.wallGrind.Stop(true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        vehicleMechanics.frictionScale = 0.4f;
        vehicleMechanics.wallGrind.Stop(true);
    }
}
