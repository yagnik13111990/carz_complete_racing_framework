using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDriveHandle 
{ 
    float MotorTorque {  get; set; }
    float SteerAngle {  get; set; }
    float BrakeTorque {  get; set; }
    float DownForce {  get; set; }
    float BrakeInput {  get; set; }
    float MotorRPM { get; set; }
    float DriftFactor {  get; set; }

    List<WheelFrictionCurve> ForwardFrictions { get; set; }

    List<WheelFrictionCurve> SidewayFrictions { get; set; }


    void ApplyThrottle();
    void ApplySteer();
    void ApplyBrakes();
   
    void ApplyFrictions();
    Rigidbody RB { get; set; }
}
