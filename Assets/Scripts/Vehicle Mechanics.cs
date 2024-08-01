// Code Reference: Yeisonlop10 (2019) Hover-racer/scripts/vehiclemovement.cs at master · Yeisonlop10/hover-racer, GitHub. Available at: https://github.com/Yeisonlop10/Hover-Racer/blob/master/Scripts/VehicleMovement.cs (Accessed: 01 August 2024). 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleMechanics : MonoBehaviour
{
    public float currentSpeed; // Current speed of the vehicle

    [Header("Vehicle Movement Settings:")]
    [SerializeField] float thrusterForce;       // Force of the vehicle's 'engine'
    [SerializeField] float slowingVelFactor;    // How much the vehicle slows when no thrust input is given (vehicle velocity is reduced by 1% per frame)
    [SerializeField] float brakingVelFactor;    // How much the vehicle slows when braking is applied (vehicle velocity is reduced by 5% per frame)
    [SerializeField] float bankAngle;         // How much the vehicle banks when turning (for visual purposes)

    [Header("Hover Settings:")]
    [SerializeField] float hoverHeight;         // The height the vehicle should consistantly maintain above the ground
    [SerializeField] float maxDistFromGround;       // Height above the ground before the vehicle is considered to be airborn
    [SerializeField] float hoverForce;          // The force of the ship's hovering system
    [SerializeField] LayerMask groundLayer;     // Determine the ground layer
    [SerializeField] PIDController pidController; // Reference to the PID Controller class to smooth the vehicle's hovering

    [Header("Custom Physics:")]
    [SerializeField] Transform vehicleBody;     // A reference to the vehicle's body
    [SerializeField] float maxSpeed;            // The maximum speed the vehicle can reach (unboosted)
    [SerializeField] float downforce;             // The downward force applied when the vehicle is 'grounded'
    [SerializeField] float airbornDownforce;         // The downward force applied when the vehicle is airborn

    Rigidbody rb;               // Reference to the vehicle's rigidbody
    PlayerInput playerInput;    // Reference to the player input class
    float drag;                 // Air resisitance to the vehicle's thrust
    bool grounded;              // Boolean to determine if the vehicle is on the ground, or airborn

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();

        // Calculate the vehicle's drag
        drag = thrusterForce / maxSpeed;
    }

    void FixedUpdate()
    {
        // Calculate the vehicle's current 'forward' speed by determining the dot product of the rigidbody velocity and its
        // positive z-axis vector
        currentSpeed = Vector3.Dot(rb.velocity, transform.forward);

        // Call the helper functions to determine the forces which should be added to the vehicle for movement and hovering
        CalcHover();
        CalcMovement();
    }

    void CalcHover()
    {
        Vector3 groundNormal;

        Ray ray = new Ray(transform.position, -transform.up);

        RaycastHit hit;

        grounded = Physics.Raycast(ray, out hit, maxDistFromGround, groundLayer);

        if (grounded)
        {
            float height = hit.distance;

            groundNormal = hit.normal.normalized;

            float forcePercent = pidController.CalcPID(hoverHeight, height);

            Vector3 force = groundNormal * hoverForce * forcePercent;

            Vector3 gravity = -groundNormal * downforce * height;

            rb.AddForce(force, ForceMode.Acceleration);
            rb.AddForce(gravity, ForceMode.Acceleration);
        }
        else 
        { 
            groundNormal = Vector3.up;

            Vector3 gravity = -groundNormal * airbornDownforce;
            rb.AddForce(gravity, ForceMode.Acceleration);
        }

        Vector3 projection = Vector3.ProjectOnPlane(transform.forward, groundNormal);
        Quaternion rotation = Quaternion.LookRotation(projection, groundNormal);

        rb.MoveRotation(Quaternion.Lerp(rb.rotation, rotation, Time.deltaTime * 10f));

        float angle = bankAngle * -playerInput.rudder;

        Quaternion bodyRotation = transform.rotation * Quaternion.Euler(0f, 0f, angle);
        vehicleBody.rotation = Quaternion.Lerp(vehicleBody.rotation, bodyRotation, Time.deltaTime * 10f);
    }

    void CalcMovement()
    {
        float rotationTorque = playerInput.rudder - rb.angularVelocity.y;

        rb.AddRelativeTorque(0f, rotationTorque, 0f, ForceMode.VelocityChange);

        float sidewaysSpeed = Vector3.Dot(rb.velocity, transform.right);

        Vector3 sidewaysFriction = -transform.right * (sidewaysSpeed / Time.fixedDeltaTime);

        rb.AddForce(sidewaysFriction, ForceMode.Acceleration);

        if (playerInput.thruster <= 0)
        {
            rb.velocity *= slowingVelFactor; 
        }

        //if (!grounded)
        //{
        //    return;
        //}

        //if (playerInput.isBraking)
        //{
        //    rb.velocity *= brakingVelFactor;
        //}

        float thrust = thrusterForce * playerInput.thruster - drag * Mathf.Clamp(currentSpeed, 0f, maxSpeed);
        rb.AddForce(transform.forward * thrust, ForceMode.Acceleration);
    }

    public float GetSpeedPercentage()
    {
        return rb.velocity.magnitude / maxSpeed;
    }
}
