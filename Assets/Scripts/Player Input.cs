using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // Player input for forward and backward movement
    [HideInInspector] public float thruster;
    // Player input for turning movement (rotation on vehicle y-axis)
    [HideInInspector] public float rudder;

    // Update is called once per frame
    void Update()
    {
        // Get the values for vehicle movement from player input
        thruster = Input.GetAxis("Vertical");
        rudder = Input.GetAxis("Horizontal");
    }
}
