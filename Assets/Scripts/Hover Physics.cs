// Tutorial: https://www.youtube.com/watch?v=j9Cl_L0zki0

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverPhysics : MonoBehaviour
{
    // Vehicle's Rigidbody
    Rigidbody rb_vehicle;

    // Vehicle movement variables
    [Header("Vehicle Parameters")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnSpeed;

    // Spring (Suspention Settings)
    [Header("Spring/Hover Parameters")]
    // Spring objects
    [SerializeField] private List<GameObject> springs;
    // Spring strength / Anti-gravity force
    [SerializeField] private float strength;
    // Spring length / Height from ground
    [SerializeField] private float length;
    // Spring dampening
    [SerializeField] private float dampening;
    // Spring compression from previous frame
    [SerializeField] private float lastHitDist;

    void Start()
    {
        rb_vehicle = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Add force relative to the forward postion of the vehicle to move the vehicle forward (top speed = 4000)
        rb_vehicle.AddForce(moveSpeed * transform.forward * Input.GetAxis("Vertical") * Time.deltaTime);
        // Add torque to y-axis to turn the vehicle 
        rb_vehicle.AddTorque(turnSpeed * transform.TransformDirection(Vector3.up) * Input.GetAxis("Horizontal") * Time.deltaTime);
    }

    void FixedUpdate()
    {
        foreach (GameObject spring in springs) 
        {
            RaycastHit hit;

            // Check if the raycast/spring is within its length to the 'ground'
            if (Physics.Raycast(spring.transform.position, transform.TransformDirection(Vector3.down), out hit, length))
            {
                // Determine the amount of force that should be applied to make the vehicle hover (dampened using Hooke's Law)
                float forceAmount = HookesLawDampen(hit.distance);

                // Add the dampended force to the spring
                rb_vehicle.AddForceAtPosition(transform.up * forceAmount, spring.transform.position);
            }
            else
            {
                // If the raycast is not within the specified length to the ground, essentially reset the compression on the spring
                lastHitDist = length * 1.1f;
            }
        }
    }

    private float HookesLawDampen(float hitDistance)
    {
        float forceAmount = strength * (length - hitDistance) + (dampening * (lastHitDist - hitDistance));
        forceAmount = Mathf.Max(0f, forceAmount);
        lastHitDist = hitDistance;

        return forceAmount;
    }
}
