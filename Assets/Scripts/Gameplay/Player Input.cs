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
    // Player input for firing weapon
    [HideInInspector] public bool fireWeapon;
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

    // Update is called once per frame
    void Update()
    {
        // Option to exit the application
        //if (Input.GetKey(KeyCode.Escape) && !Application.isEditor) { Application.Quit(); }

        // Disable vehicle controls if the game manager exists and the game is over
        if (GameManager.instance != null && !GameManager.instance.GameIsActive())
        {
            thruster = 0f;
            yaw = 0f;
            return;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Get the values for vehicle movement from player gamepad input
        float forwardThrust = Input.GetAxis("Accelerate");
        float backwardThrust = Input.GetAxis("Decelerate");

        // Check if gamepad inputs have values, then apply thruster
        if (forwardThrust > 0 || backwardThrust > 0) { thruster = forwardThrust + -backwardThrust; }
        // If no gamepad input is given, use keyboard axis for thrust
        else thruster = Input.GetAxis("Vertical");

        // Add increased braking force
        if (thruster < 0) { thruster *= 1.25f; }

        //Debug.Log(thruster);
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // Get horizontal input for turning (yaw rotation)
        yaw = Input.GetAxis("Horizontal");

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        boost = Input.GetButton("Boost"); //|| Input.GetButton("Boost2");

        // Play boost sound
        if (Input.GetButtonDown("Boost")) //|| Input.GetButtonDown("Boost2"))
        {
            GameManager.instance.sfxManager.PlayBoostSFX();
        }

        if (GameManager.instance.gameMode == GameManager.GameMode.PilotGauntlet)
        {
            if ((Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2") || Input.GetButtonDown("Fire3")) && !GameManager.instance.gameIsPaused)
            {
                GameManager.instance.sfxManager.PlayFireWeaponSFX();
            }

            fireWeapon = Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2") || Input.GetButtonDown("Fire3");
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
