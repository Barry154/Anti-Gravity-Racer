// This script manages the amount of hover force to apply to the vehicle using the PID (proportional, integral, derivative) algorithm.

// This script is based on code and techniques taught in the Cybernetic Walrus workshop hosted by UnityEDU and PID controller tutorial by Vazgriz on YouTube.
// The algorithm and implementation was mostly learnt from the UnityEDU workshop, however has been amended to fit certain practises detailed by Vazgriz.
// Therefore, amendments have been marked with comments.
// withing the code as well. 
// Workshop YouTube link: https://www.youtube.com/watch?v=ULDhOuU2JPY&list=PLX2vGYjWbI0SvPiKiMOcj_z9zCG7V9lkp&index=1
// UnityEDU GitHub repo link for code file (PIDController): https://github.com/Yeisonlop10/Hover-Racer/blob/master/Scripts/PIDController.cs
// Vazgriz YouTube tutorial link: https://www.youtube.com/watch?v=y3K6FUgrgXw&t=209s

using UnityEngine;

[System.Serializable]
public class PIDController
{
    ////////////////////////////////////////////UnityEDU Code Start (PIDController)/////////////////////////////////////////
    // PID Coefficient/Gain values
    public float pGain = 0.8f;
    public float iGain = 0.0002f;
    public float dGain = 0.2f;

    // Output range
    public float minOutput = -1;
    public float maxOutput = 1;
    ////////////////////////////////////////////UnityEDU Code End (PIDController)///////////////////////////////////////////

    // Initialise a variable to store the sum of the error multiplied by delta time. This is used to increase or decrease the
    // integral term over time as needed
    public float sumIntergration; // Vazgriz inspired amendment

    // Initialise a variable to store the error from the previous frame, as this is used to calculate the derivative
    public float prevError; // Vazgriz inspired amendment

    // Check if the derivative has been initialised. This will only be true from frame 2 onward to prevent derivative kick 
    // on the first frame when the previous error = 0
    public bool derivativeInitialised; // Vazgriz inspired amendment

    ////////////////Vazgriz Amendment Start/////////////////
    // Called to reset the derivative when the game restarts
    public void Reset()
    {
        derivativeInitialised = false;
    }
    ////////////////Vazgriz Amendment End/////////////////

    // Function which performs the PID algorithm calculation
    public float CalcPID(float targetValue, float currentValue)
    {
        float deltaTime = Time.fixedDeltaTime; // UnityEDU (PIDController)
        // Error dictates how 'far away' the current value is from the target. 
        // This is used to determine how much force should be added to the object to reach the target value, as well as the direction.
        float error = targetValue - currentValue; // Vazgriz inspired amendment (uses more conventional terminology)

        // P term, calculated by multiplying its gain by the error to determine size and direction of the applied force
        float proportional = pGain * error; // Vazgriz inspired amendment (uses more conventional implementation of P term)

        ////////////////Vazgriz Amendment Start/////////////////
        // D term, acts as a dampening force to the P term to prevent overshoot. Uses the rate of change of the error to apply the 
        // corresponding dampening force to P
        float derivativeEROC = 0;
        float errorRateOfChange = (error - prevError) / deltaTime; // Calculate the rate of change of the error
        prevError = error; // Store previous error
        if (derivativeInitialised) { derivativeEROC = errorRateOfChange; } // Check if derivative has been initialised, then store error rate of change to be used
        else { derivativeInitialised = true; } // Set derivativeInitialised to true if still false 
        float derivative = dGain * derivativeEROC;
        ////////////////Vazgriz Amendment End/////////////////

        // I term is generally used to counter for some external force which causes the object to not reach the target value, even though 
        // the P term is 0 (such as gravity). The sum of the error and delta time from each frame is stored and used to find the 'balancing' 
        // force, I
        sumIntergration += error * deltaTime; // Vazgriz inspired amendment (uses more conventional implementation of I term)
        float integral = iGain * sumIntergration; // Vazgriz inspired amendment (uses more conventional implementation of I term)

        // Sum the terms together to finish the PID algorithm
        float value = proportional + integral + derivative; // UnityEDU (PIDController) - small amendment as the coefficients have already been included
        // Clamp the calculated value within the specified range
        value = Mathf.Clamp(value, minOutput, maxOutput); // UnityEDU (PIDController)

        return value;
    }
}
