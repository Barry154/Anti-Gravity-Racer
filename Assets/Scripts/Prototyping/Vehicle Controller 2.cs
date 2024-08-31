// Tutorial: https://www.youtube.com/watch?v=qsfIXopyYHY

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VehicleController2 : MonoBehaviour
{
    // Vehicle Rigidbody
    private Rigidbody rb;

    // Reference variable
    private float rotVelocity;

    public List<GameObject> thrusters;
    
    //public GameObject engine;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Add force relative to the forward postion of the vehicle to move the vehicle forward (top speed = 4000)
        rb.AddForce(Time.deltaTime * transform.forward * Input.GetAxis("Vertical") * 4000f);
        // Add torque to y-axis to turn the vehicle 
        rb.AddTorque(Time.deltaTime * transform.TransformDirection(Vector3.up) * Input.GetAxis("Horizontal") * 300f);

        // Hovering physics using raycasts
        foreach (GameObject t in thrusters) 
        {
            RaycastHit hit;
            if (Physics.Raycast(t.transform.position, transform.TransformDirection(Vector3.down), out hit, 3f))
            {
                // Uses inverse square law to scale the force applied when calculating raycast distance
                //rb.AddForceAtPosition(Time.deltaTime * transform.TransformDirection(Vector3.up) * Mathf.Pow(3f - hit.distance, 2) / 3f * 250f, t.transform.position);

                // Attempting to smooth the raycast hit distance changes (somewhat like lerp)
                Vector3 thrusterForceScaled = Time.deltaTime * transform.TransformDirection(Vector3.up) * Mathf.Pow(3f - hit.distance, 2) / 3f * 250f;
                rb.AddForceAtPosition(new Vector3(0, Mathf.SmoothStep(0f, 250f, thrusterForceScaled.y), 0), t.transform.position);
            }
            Debug.Log(hit.distance);
        }

        rb.AddForce(-Time.deltaTime * transform.TransformVector(Vector3.right) * transform.InverseTransformVector(rb.velocity).x * 5f);

        // 'Fake' rotate the vehicle when turning
        //Vector3 newRotation = transform.eulerAngles;
        //newRotation.z = Mathf.SmoothDampAngle(newRotation.z, Input.GetAxis("Horizontal") * -25f, ref rotVelocity, 0.25f);
        //transform.eulerAngles = newRotation;
    }
}
