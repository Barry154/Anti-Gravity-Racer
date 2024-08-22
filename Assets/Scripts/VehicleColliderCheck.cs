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

        if (collision.gameObject.CompareTag("Wall"))
        {
            float collisionForce = (Mathf.Abs(vehicleMechanics.currentSpeed) / 5);
            GameManager.instance.CheckVehicleCollision(collisionForce);
            //Debug.Log(collisionForce);
        }

        if (collision.gameObject.CompareTag("Mine"))
        {
            Destroy(collision.gameObject);
            GameManager.instance.CheckVehicleCollision(250);
        }
    }
}
