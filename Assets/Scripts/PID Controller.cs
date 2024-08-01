// 

using UnityEngine;

[System.Serializable]
public class PIDController
{
    // PID Coefficient/Gain values
    public float pGain = 0.8f;
    public float iGain = 0.0002f;
    public float dGain = 0.2f;

    // Output range
    public float minOutput = -1;
    public float maxOutput = 1;

    // Initialise a variable to store the sum of the error multiplied by delta time. This is used to increase or decrease the
    // integral term over time as needed
    public float sumIntergration;

    // Initialise a variable to store the error from the previous frame, as this is used to calculate the derivative
    public float prevError;

    // Check if the derivative has been initialised. This will only be true from fram 2 onward to prevent derivative kick 
    // on the first frame when the previous error = 0
    public bool derivativeInitialised;

    // Called to reset the derivative when the game restarts
    public void Reset()
    {
        derivativeInitialised = false;
    }

    // Function which performs the PID algorithm calculation
    public float CalcPID(float targetValue, float currentValue)
    {
        float deltaTime = Time.fixedDeltaTime;
        // Error dictates how 'far away' the current value is from the target. 
        // This is used to determine how much force should be added to the object to reach the target value, as well as the direction.
        float error = targetValue - currentValue;

        // P term, calculated by multiplying its gain by the error to determine size and direction of the applied force
        float proportional = pGain * error;

        // D term, acts as a dampening force to the P term to prevent overshoot. Uses the rate of change of the error to apply the 
        // corresponding dampening force to P
        float derivativeEROC = 0;
        float errorRateOfChange = (error - prevError) / deltaTime; // Calculate the rate of change of the error
        prevError = error; // Store previous error
        if (derivativeInitialised) { derivativeEROC = errorRateOfChange; } // Check if derivative has been initialised, then store error rate of change to be used
        else { derivativeInitialised = true; } // Set derivativeInitialised to true if still false 
        float derivative = dGain * derivativeEROC;

        // I term is generally used to counter for some external force which causes the object to not reach the target value, even though 
        // the P term is 0 (such as gravity). The sum of the error and delta time from each frame is stored and used to find the 'balancing' 
        // force, I
        sumIntergration += error * deltaTime;
        float integral = iGain * sumIntergration;
        
        // Sum the terms together to finish the PID algorithm
        float value = proportional + integral + derivative;
        // Clamp the calculated value within the specified range
        value = Mathf.Clamp(value, minOutput, maxOutput);

        return value;
    }
}
