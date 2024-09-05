// This script receives and maps all the player inputs

// This script uses some code from the player controller script from the Cybernetic Walrus workshop hosted by UnityEDU. This section has bee marked with
// 'start' and 'end' comments.
// Workshop YouTube link: https://www.youtube.com/watch?v=ULDhOuU2JPY&list=PLX2vGYjWbI0SvPiKiMOcj_z9zCG7V9lkp&index=1
// GitHub repo link for code file (PlayerInput): https://github.com/Yeisonlop10/Hover-Racer/blob/master/Scripts/PlayerInput.cs

using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // Player input for forward and backward movement
    [HideInInspector] public float thruster;
    // Player input for turning movement (rotation on vehicle y-axis)
    [HideInInspector] public float yaw;
    // Player input for boosting
    [HideInInspector] public bool boost;
    // Player input for firing weapon
    [HideInInspector] public bool fireWeapon;

    // Update is called once per frame
    void Update()
    {
        ////////////////////////////////////////////UnityEDU Code Start (PlayerInput)/////////////////////////////////////////
        // Disable vehicle controls if the game manager exists and the game is over
        if (GameManager.instance != null && !GameManager.instance.GameIsActive())
        {
            thruster = 0f;
            yaw = 0f;
            return;
        }
        ////////////////////////////////////////////UnityEDU Code End (PlayerInput)///////////////////////////////////////////

        // Get the values for vehicle movement from player gamepad input
        float forwardThrust = Input.GetAxis("Accelerate");
        float backwardThrust = Input.GetAxis("Decelerate");

        // Check if gamepad inputs have values, then apply thruster
        if (forwardThrust > 0 || backwardThrust > 0) { thruster = forwardThrust + -backwardThrust; }
        // If no gamepad input is given, use keyboard axis for thrust
        else thruster = Input.GetAxis("Vertical");

        // Add increased braking force
        if (thruster < 0) { thruster *= 1.25f; }

        // Get horizontal input for turning (yaw rotation)
        yaw = Input.GetAxis("Horizontal");

        // Get input for boost
        boost = Input.GetButton("Boost");

        // Play boost sound if the boost input is received
        if (Input.GetButtonDown("Boost"))
        {
            GameManager.instance.sfxManager.PlayBoostSFX();
        }

        // Get the weapon fire input if the game mode enum is 'pilots gauntlet', the input key is not 'right alt', and the game is not paused (the latter two conditions are to prevent bugs)
        if ((GameManager.instance.gameMode == GameManager.GameMode.PilotGauntlet) && !Input.GetKey(KeyCode.RightAlt) && !GameManager.instance.gameIsPaused)
        {
            fireWeapon = Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2") || Input.GetButtonDown("Fire3");
        }
    }
}
