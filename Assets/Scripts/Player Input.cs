using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // Player input for forward and backward movement
    [HideInInspector] public float thruster;
    // Player input for turning movement (rotation on vehicle y-axis)
    [HideInInspector] public float yaw;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Player input for boosting
    [HideInInspector] public bool boost;
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

    // Update is called once per frame
    void Update()
    {
        // Option to exit the application
        if (Input.GetKey(KeyCode.Escape) && !Application.isEditor) { Application.Quit(); }

        // Disable vehicle controls if the game manager exists and the game is over
        if (GameManager.instance != null && !GameManager.instance.GameIsActive())
        {
            thruster = 0f;
            yaw = 0f;
            return;
        }

        // Get the values for vehicle movement from player gamepad input
        float forwardThrust = Input.GetAxis("Accelerate");
        float backwardThrust = Input.GetAxis("Decelerate");

        // Check if gamepad inputs have values, then apply thruster
        if (forwardThrust > 0 || backwardThrust > 0) { thruster = forwardThrust + -backwardThrust; }
        // If no gamepad input is given, use keyboard axis for thrust
        else thruster = Input.GetAxis("Vertical");

        // Get horizontal input for turning (yaw rotation)
        yaw = Input.GetAxis("Horizontal"); 

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        boost = Input.GetButton("Boost");
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
