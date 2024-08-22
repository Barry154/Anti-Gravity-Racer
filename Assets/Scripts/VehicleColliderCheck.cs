using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleColliderCheck : MonoBehaviour
{
    [Header("Vehicle Mechanics:")]
    [SerializeField] VehicleMechanics vehicleMechanics;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);

        if (collision.gameObject.CompareTag("Wall"))
        {
            float collisionForce = (Mathf.Abs(vehicleMechanics.currentSpeed) / 3);
            GameManager.instance.CheckVehicleCollision(collisionForce);
            //GameManager.instance.CheckVehicleCollision(100);
        }

        if (collision.gameObject.CompareTag("Mine"))
        {
            GameManager.instance.CheckVehicleCollision(250);
        }
    }
}
