// This script controls the vehicle physics. This regards the vehicle's hover mechanics, as well as movement. This script also controls the engine SFX
// for the vehicle when moving, as well as vehicle destruction when durability reaches 0.

// This script is based on code and techniques taught in the Cybernetic Walrus workshop hosted by UnityEDU and has been altered
// and includes multiple original additions for this project. Sections which were taken from the UnityEDU code are marked with
// 'start' and 'end' comments. All other code is my own.
// Workshop YouTube link: https://www.youtube.com/watch?v=ULDhOuU2JPY&list=PLX2vGYjWbI0SvPiKiMOcj_z9zCG7V9lkp&index=1
// GitHub repo link for code file (VehicleMovement): https://github.com/Yeisonlop10/Hover-Racer/blob/master/Scripts/VehicleMovement.cs

using System.Collections;
using UnityEngine;

public class VehicleMechanics : MonoBehaviour
{
    public float currentSpeed; // Current speed of the vehicle

    [Header("Vehicle Movement Settings:")]
    [SerializeField] float thrusterForce;       // Force of the vehicle's 'engine'
    [SerializeField] float boostForce;          // Force of the vehicle's engine when boosting
    [SerializeField] float maxSpeed;            // The maximum speed the vehicle can reach (unboosted)
    [SerializeField] float boostMaxSpeed;       // The maximum speed the vehicle can reach (boosted)
    [SerializeField] float slowingVelFactor;    // How much the vehicle slows when no thrust input is given (vehicle velocity is reduced by 1% per frame)
    [SerializeField] float bankAngle;           // How much the vehicle banks when turning (for visual purposes)

    [Header("Hover Settings:")]
    [SerializeField] float hoverHeight;             // The height the vehicle should consistantly maintain above the ground
    [SerializeField] float maxDistFromGround;       // Height above the ground before the vehicle is considered to be airborn
    [SerializeField] float hoverForce;              // The force of the ship's hovering system
    [SerializeField] LayerMask groundLayer;         // Determine the ground layer
    public PIDController pidController;             // Reference to the PID Controller class to smooth the vehicle's hovering

    [Header("Custom Physics:")]
    [SerializeField] float downforce;               // The downward force applied when the vehicle is 'grounded'
    [SerializeField] float airbornDownforce;        // The downward force applied when the vehicle is airborn
    [SerializeField] public float frictionScale;    // The fraction which is multiplied to the sideways/lateral friction of the vehicle (lower values allow more drift)

    [Header("Vehicle Components")]
    [SerializeField] Transform vehicleBody;         // A reference to the vehicle's body
    [SerializeField] GameObject vehicleColliders;   // A reference to the vehicle's colliders

    [Header("Particle Systems (Afterburner and Damage)")]
    [SerializeField] ParticleSystem afterburner;            // Reference to the particle system for the vehicle's 'afterburner'
    private ParticleSystem.MainModule afterburnerModule;    // Reference to the main module of the afterburner particle system, so that values on the module can be modified
    [SerializeField] public ParticleSystem smoke;           // Reference to the particle effect which simulates smoke
    [SerializeField] public ParticleSystem damageSparks;    // Reference to the particle effect which simulates vehicle damage sparks
    [SerializeField] public ParticleSystem wallGrind;       // Reference to the particle effect which simulates collision sparks

    [Header("Particle Systems (Explosion) Game Object")]
    [SerializeField] GameObject explosion;                  // Reference to the game object which contains all the explosion particle effects
    [SerializeField] float waitTime = 1;                    // Floating point value which represents a delay time

    [Header("Light")]
    [SerializeField] Light light;                   // Reference to the point light which is part of the vehicle's afterburner

    [Header("Engine Audio Source")]
    [SerializeField] AudioSource engineSound;       // Reference to the audio source which plays the engine hum

    [HideInInspector] public Rigidbody rb;          // Reference to the vehicle's rigidbody
    PlayerInput playerInput;                        // Reference to the player input class
    float drag;                                     // Air resisitance to the vehicle's thrust
    bool grounded;                                  // Boolean to determine if the vehicle is on the ground, or airborn

    [HideInInspector] public bool canBoost;         // Boolean which determines if the vehicle can use the boost module
    [HideInInspector] public bool isBoosting;       // Boolean which determines if the boost module is currently in use

    void Start()
    {
        ////////////////////////////////////////////UnityEDU Code Start (VehicleMovement)/////////////////////////////////////////
        //Get references to the Rigidbody and PlayerInput components
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();

        // Calculate the vehicle's drag
        drag = thrusterForce / maxSpeed;
        ////////////////////////////////////////////UnityEDU Code End (VehicleMovement)///////////////////////////////////////////


        // Initialise the particle effect module for the afterburner
        afterburnerModule = afterburner.main;
        // Stop the playback of particle effects which concern vehicle damage
        smoke.Stop();
        damageSparks.Stop();
        wallGrind.Stop();
    }

    void FixedUpdate()
    {
        ////////////////////////////////////////////UnityEDU Code Start (VehicleMovement)/////////////////////////////////////////
        // Calculate the vehicle's current 'forward' speed by determining the dot product of the rigidbody velocity and its
        // positive z-axis vector
        currentSpeed = Vector3.Dot(rb.velocity, transform.forward);

        // Call the helper functions to determine the forces which should be added to the vehicle for movement and hovering
        CalcHover();
        CalcMovement();
        ////////////////////////////////////////////UnityEDU Code End (VehicleMovement)///////////////////////////////////////////

        // Call the function which changes the engine sound's pitch
        ChangeEnginePitch();
    }

    ////////////////////////////////////////////UnityEDU Code Start (VehicleMovement)/////////////////////////////////////////
    void CalcHover()
    {
        // 'Container' for the surface normal of the track's surface
        Vector3 groundNormal;

        // The ray which points directly down from the vehicle
        Ray ray = new Ray(transform.position, -transform.up);

        // Variable which will hold the result of a raycast
        RaycastHit hit;

        // Determines if the vehicle is 'on the ground' by checking if the raycast from the vehicle is 'touching' the surface with
        // the LayerMask of 'Ground', as well as being within the maximum distance
        grounded = Physics.Raycast(ray, out hit, maxDistFromGround, groundLayer);

        // If the vehicle is 'on the ground'
        if (grounded)
        {
            // Check how high off the ground the vehicle is
            float height = hit.distance;

            // Store the normal of the track surface
            groundNormal = hit.normal.normalized;

            // Calculate the amount of hover force needed to maintain the target height distace, using the PID controller
            float forcePercent = pidController.CalcPID(hoverHeight, height);

            // Calculate the total amount of hover force based on the track's surface normal
            Vector3 force = groundNormal * hoverForce * forcePercent;

            // Calculate the relative downforce which opposes the hover force and keeps the vehicle close to the track based on the
            // direction of the ground normal (this is the 'custom gravity')
            Vector3 gravity = -groundNormal * downforce * height;

            // Add the respective hover and downward forces to the vehicle
            rb.AddForce(force, ForceMode.Acceleration);
            rb.AddForce(gravity, ForceMode.Acceleration);
        }
        // Otherwise, the vehicle is airborn
        else 
        { 
            // Use 'Vector3.up' to represent the 'surface normal'. Works in the context in this game because of the angle of the jump.
            groundNormal = Vector3.up;

            // Calculate and use the stronger downward force to bring the vehicle back to the ground
            Vector3 gravity = -groundNormal * airbornDownforce;
            rb.AddForce(gravity, ForceMode.Acceleration);
        }

        // Calculate the amount of pitch and roll (x and z axis rotation) the vehicle needs to match its orientation
        // with the track surface. This is calculated by creating a projection and then calculating
        // the rotation needed to face that projection
        Vector3 projection = Vector3.ProjectOnPlane(transform.forward, groundNormal);
        Quaternion rotation = Quaternion.LookRotation(projection, groundNormal);

        // Move the ship over time (lerp) to match the desired rotation to match the ground
        rb.MoveRotation(Quaternion.Lerp(rb.rotation, rotation, Time.fixedDeltaTime * 10f));

        // Calculate the angle the vehicle's body should bank when turning, based on the current turn input (yaw)
        float angle = bankAngle * -playerInput.yaw;

        // Calculate the rotation needed for banking
        Quaternion bodyRotation = transform.rotation * Quaternion.Euler(0f, 0f, angle);
        // Apply this angle to the vehicle's body
        vehicleBody.rotation = Quaternion.Lerp(vehicleBody.rotation, bodyRotation, Time.fixedDeltaTime * 10f);
    }
    ////////////////////////////////////////////UnityEDU Code End (VehicleMovement)///////////////////////////////////////////

    void CalcMovement()
    {
        // Floating point value that will store the amount of turning torque to add to the vehicle
        float rotationTorque;

        // If the vehicle is moving forward (positive velocity), calculate the torque based on the player input and rigidbody angular velocity
        if (currentSpeed >= 0) { rotationTorque = playerInput.yaw - rb.angularVelocity.y; }
        // If the vehicle is moving backward (negative velocity), calculate the torque based on the inversed player input and rigidbody angular velocity
        else { rotationTorque = -playerInput.yaw - rb.angularVelocity.y; }

        ////////////////////////////////////////////UnityEDU Code Start (VehicleMovement)/////////////////////////////////////////
        // Apply the torque to the vehicle on the y-axis
        rb.AddRelativeTorque(0f, rotationTorque, 0f, ForceMode.VelocityChange);

        // Calculate the amount of velocity on the vehicle's x-axis (how much it is drifting sideways; the inertial drift)
        float sidewaysSpeed = Vector3.Dot(rb.velocity, transform.right);

        // Calculate the opposing force to 'sidewaysSpeed' to control the amount of grip the vehicle has when turning
        Vector3 sidewaysFriction = (-transform.right * (sidewaysSpeed / Time.fixedDeltaTime)) * frictionScale; // This line of code was amended to include some inertial drift in the handeling of the vehicle

        // Apply this sideways force to the vehicle
        rb.AddForce(sidewaysFriction, ForceMode.Acceleration);

        // If no input is received from the player
        if (playerInput.thruster == 0) // Amended so that the vehicle slows when no input is given, but may also reverse
        {
            // Slow the vehicle by a desired amount
            rb.velocity *= slowingVelFactor; 
        }
        ////////////////////////////////////////////UnityEDU Code End (VehicleMovement)///////////////////////////////////////////

        // If the vehicle is boosting
        if (canBoost && playerInput.boost)
        {
            // Vehicle is currently using boost
            isBoosting = true;

            // The thrust force is calculated, and scaled and clamped by the boost variables from the vehicle control settings
            float thrust = boostForce * 1 - drag * Mathf.Clamp(currentSpeed, 0f, boostMaxSpeed); // Amended from UnityEDU VehicleMovement script
            // Add the boost thrust to the vehicle
            rb.AddForce(transform.forward * thrust, ForceMode.Acceleration);

            // Set the afterburner particle system (and point light) to a specific colour and length to visually relay to the player that the vehicle is boosting
            afterburnerModule.startColor = Color.green;
            afterburnerModule.startLifetime = 1f;
            light.color = Color.green;
        }
        else
        {
            // Vehicle is not currently using boost
            isBoosting = false;

            // Set the afterburner particle system to a the 'regular' colour
            afterburnerModule.startColor = Color.magenta;
            // Set the length of the afterburner particle system to a value based on the current speed, scaled to a range between 0.4 and 1
            float flameLength = ((currentSpeed - 0f) * (1f - 0.4f) / (maxSpeed - 0f)) + 0.4f;
            // Set the afterburner length
            afterburnerModule.startLifetime = flameLength;
            // Set the point light to the 'regular' colour
            light.color = Color.magenta;

            // Taper the vehicle's speed so as to not exceed the specified maximum speed. Also, taper the reversing speed to be no more than 10m/s
            if (currentSpeed > maxSpeed || currentSpeed < -10)
            {
                rb.velocity *= slowingVelFactor;
            }
            // Otherwise, if the respective top speeds have not yet been reached
            else
            {
                // Calculate the amount of thrust force to apply to the vehicle, taking drag into consideration
                float thrust = thrusterForce * playerInput.thruster - drag * Mathf.Clamp(currentSpeed, 0f, maxSpeed); // UnityEDU VehicleMovement script
                // Add the force to the vehicle
                rb.AddForce(transform.forward * thrust, ForceMode.Acceleration);  
            }
        }
    }

    ////////////////////////////////////////////UnityEDU Code Start (VehicleMovement)/////////////////////////////////////////
    public float GetSpeedPercentage()
    {
        return rb.velocity.magnitude / maxSpeed;
    }
    ////////////////////////////////////////////UnityEDU Code End (VehicleMovement)///////////////////////////////////////////

    // Coroutine which is called when the vehicle's durability is 0. This starts the process of destroying the vehicle.
    public IEnumerator DestroyVehicle(float waitTime)
    {
        // Stop the vehicle's movement
        rb.velocity = Vector3.zero;

        // Deactivate the vehicle's body models
        vehicleBody.gameObject.SetActive(false);
        // Deactivate the vehicle's colliders
        vehicleColliders.SetActive(false);

        // Activate the game object which contains the explosion particle systems
        explosion.SetActive(true);

        // Wait for the particle systems to finish playing
        yield return new WaitForSeconds(waitTime);

        // Destroy the vehicle
        Destroy(gameObject);
    }

    // Changes the pitch of the audio clip on the audio source which plays the engine hum
    void ChangeEnginePitch()
    {
        // Scale the current speed to a value between 0 and 0.5
        float pitchAdjust = ((currentSpeed - 0f) * (0.5f - 0f) / (maxSpeed - 0f)) + 0f;
        // Set the pitch of the audio clip to the value of the above calculation. This way, the vehicle's engine sound will change depending on how fast the vehicle is moving
        engineSound.pitch = pitchAdjust;
    }
}
