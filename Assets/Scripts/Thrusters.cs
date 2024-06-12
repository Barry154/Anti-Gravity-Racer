// Tutorial: https://www.youtube.com/watch?v=r9OEZmbD9q0

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrusters : MonoBehaviour
{
    public float thrusterStrength;
    public float thrusterDistance;
    public Transform[] thrusters;
    Rigidbody rb;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        foreach (Transform t in thrusters)
        {
            Vector3 downwardForce;
            float distancePercentage;

            if(Physics.Raycast (t.position, t.up * -1, out hit, thrusterDistance))
            {
                // The thruster is within thrusterDistance to the ground. How far away?
                distancePercentage = 1-(hit.distance/thrusterDistance);

                // Calculate how much force to apply to the thruster
                downwardForce = transform.up * thrusterStrength * distancePercentage;
                // Correct the force for the mass of the car and deltaTime
                downwardForce = downwardForce * Time.deltaTime * rb.mass;

                // Apply the force where the thruster is
                rb.AddForceAtPosition(downwardForce, t.position);
            }
        }
    }
}
