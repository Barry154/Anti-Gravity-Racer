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
        // Disable vehicle controls if the game manager exists and the game is over
        if (GameManager.instance != null && !GameManager.instance.GameIsActive())
        {
            thruster = 0f;
            yaw = 0f;
            return;
        }

        // Get the values for vehicle movement from player input
        thruster = Input.GetAxis("Vertical");
        yaw = Input.GetAxis("Horizontal");

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        boost = Input.GetKey(KeyCode.Space);
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
