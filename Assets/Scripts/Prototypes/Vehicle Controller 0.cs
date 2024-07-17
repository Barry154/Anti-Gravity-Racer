using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VehicleController0 : MonoBehaviour
{
    // Vehicle's Rigidbody
    Rigidbody rb;

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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Add force relative to the forward postion of the vehicle to move the vehicle forward (top speed = 4000)
        rb.AddForce(Time.deltaTime * transform.forward * Input.GetAxis("Vertical") * moveSpeed);
        // Add torque to y-axis to turn the vehicle 
        rb.AddTorque(Time.deltaTime * transform.TransformDirection(Vector3.up) * Input.GetAxis("Horizontal") * turnSpeed);

        // Hovering physics using raycasts
        foreach (GameObject s in springs) 
        {
            RaycastHit hit;
            if (Physics.Raycast(s.transform.position, transform.TransformDirection(Vector3.down), out hit, length))
            {
                // Uses inverse square law to scale the force applied when calculating raycast distance
                //rb.AddForceAtPosition(Time.deltaTime * transform.TransformDirection(Vector3.up) * Mathf.Pow(length - hit.distance, 2) / length * strength, s.transform.position);

                // Attempting to smooth the raycast hit distance changes (somewhat like lerp)
                Vector3 thrusterForceScaled = Time.deltaTime * transform.TransformDirection(Vector3.up) * Mathf.Pow(length - hit.distance, 2) / length * strength;
                rb.AddForceAtPosition(new Vector3(0, Mathf.SmoothStep(0f, strength, thrusterForceScaled.y), 0), s.transform.position);
            }
            Debug.Log(hit.distance);
        }

        rb.AddForce(-Time.deltaTime * transform.TransformVector(Vector3.right) * transform.InverseTransformVector(rb.velocity).x * 5f);
    }
}
