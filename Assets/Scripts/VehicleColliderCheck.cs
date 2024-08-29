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
        //Debug.Log(collision.gameObject.tag);

        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Pillar"))
        {
            float collisionForce = (Mathf.Abs(vehicleMechanics.currentSpeed) / 5);
            GameManager.instance.CheckVehicleCollision(collisionForce);
            //Debug.Log(collisionForce);
        }

        if (collision.gameObject.CompareTag("Mine"))
        {
            Destroy(collision.gameObject);
            GameManager.instance.CheckVehicleCollision(200);
        }
    }

    private void OnCollisionStay(Collision collision) 
    {
        vehicleMechanics.wallGrind.transform.position = collision.contacts[0].point;

        if (vehicleMechanics.currentSpeed > 1.5f || vehicleMechanics.currentSpeed < -1.5f)
        {
            vehicleMechanics.wallGrind.Play(true);
        }
        
        else
        {
            vehicleMechanics.wallGrind.Stop(true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        vehicleMechanics.wallGrind.Stop(true);
    }
}
