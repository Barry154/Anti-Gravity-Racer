// Tutorial: https://www.youtube.com/watch?v=r9OEZmbD9q0

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    // Values that control the vehicle
    public float acceleration;
    public float rotationRate;

    // Values for faking a nice turn display
    public float turnRotAngle;
    public float turnRotSeekSpeed;

    // Reference variables we don't directly use
    private float rotVelocity;
    private float groundAngleVelocity;
    
    // Reference to rigidbody
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Check if the vehicle is 'touching' the ground
        if (Physics.Raycast (transform.position, transform.up * -1, 5f))
        {
            // Vehicle is on (or near to) the ground. Enable accelerator and increase drag. Increasing drag helps decelerate when no control input is given
            rb.drag = 1;

            // Calculate forward force
            Vector3 forwardForce = transform.forward * acceleration * Input.GetAxis("Vertical");
            // Correct force for deltaTime and vehicle mass
            forwardForce = forwardForce * Time.deltaTime * rb.mass;

            rb.AddForce(forwardForce);
        }
        else
        {
            // Vehicle is not on the ground. Reduce drag so that the vehicle is not stopped mid-air
            rb.drag = 0;
        }

        // Turn on the ground or mid-air
        Vector3 turnTorque = Vector3.up * rotationRate * Input.GetAxis("Horizontal");
        // Correct force for deltaTime and vehicle mass
        turnTorque = turnTorque * Time.deltaTime * rb.mass;
        rb.AddTorque(turnTorque);

        // 'Fake' rotate the vehicle when turning
        //Vector3 newRotation = transform.eulerAngles;
        //newRotation.z = Mathf.SmoothDampAngle(newRotation.z, Input.GetAxis("Horizontal") * -turnRotAngle, ref rotVelocity, turnRotSeekSpeed);
        //transform.eulerAngles = newRotation;
    }
}
