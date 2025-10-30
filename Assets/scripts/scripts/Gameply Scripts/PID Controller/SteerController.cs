using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SteerController
{
    public float ProportionalGain;  //kp
    public float IntegralGain;      //ki   
    public float DerivativeGain;    //kd

    private float Integral;
    private float PreviousError;

    public float ComputeAngle(float CurrentAngle, float TargetAngle, float deltaTime)
    {
        if (deltaTime <= 0f) deltaTime = 0.02f; // safety clamp

        float Error = TargetAngle - CurrentAngle;

        Integral += Error * deltaTime;
        Integral = Mathf.Clamp(Integral, -50f, 50f); // prevent runaway

        float Derivative = (Error - PreviousError) / deltaTime;
        Derivative = Mathf.Clamp(Derivative, -100f, 100f); // smooth sudden jumps

        float output = ProportionalGain * Error + IntegralGain * Integral + DerivativeGain * Derivative;

        PreviousError = Error;

        output = Mathf.Clamp(output, -45f, 45f); // limit steering correction
        return output;
    }

}
