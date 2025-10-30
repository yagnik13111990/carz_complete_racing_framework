using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


public struct MovementComponent : IComponentData
{
    //movement---
    public float Speed;   
    public float MaximumSpeed; //160
    public float SpeedLimit;
    public float CurrTorque;
    public float MaximumTorque;
    public float TorqueLimit;
    public float3 Velocity;
    public int Direction;

    //downforce--
    public float DownForce;
    public float AirFactor;
      
}

public struct WheelComponent : IComponentData
{

    public float MinSpeedForSteerHelp;
    public float SlipAngleThreshold;
    public float OverSteerStifnessMultiplier;
    public float RampUpStiffnessTime;
    public float HoldStiffnessTime;
    public float RestoreStiffnessTime;
    public float BaseRearStiffness;
    public float TargetRearStiffness;
    public float CurrentStiffness;
    public float HoldTimer;
    public float RampTimer;
    public float SlipAngle;
    public float AngularVelY;
 
  
    public DriftStates DriftStates;
    
}

public enum DriftStates { Idle, RampingUp, Holding, Restoring }

public struct TurnComponent : IComponentData
{
    public float MaxSteerAngle;
    public float HelpSteerPower;
    public float AngleLimitAtSpeed;
    public float SteerAngle;
    public float DriftHelper;
    public float TurnMultiplierInput;
    public float Angle;
}

public struct GearComponent : IComponentData
{
    public int CurrGear; // 0;
    public float MaximumRPM;
    public float MinimumRPM;
    public float MotorRPM;
    public float CutOffRPM;
    public float LerpSpeed;
    public float CutOffTimer;
    public float MotorTorqueOfCurve;
    public float EngineRPM;
    public bool IsEngineRPMInCutOffSituation;
    public float RPM_NextGear;
    public float RPM_PrevGear;
    public float MaxForwardSlipToAvoidChangeGear;
    public float ThrottleInput;

    public bool IsAuto;
}

public struct GearMechenismComponent : IComponentData
{
    public BlobAssetReference<GearMechenism> blob;
}


public struct GearMechenism     //bb1
{

    public BlobArray<float> GearRatios;
    public BlobArray<float> Ratios;
}

public struct BrakeComponent : IComponentData
{
    public float MaximumBrake;
    public float CurrentBrake;
    public float BrakeInput;

}



public struct DriftCar : IComponentData { }
public struct PlayerCar : IComponentData { }
public struct AICar : IComponentData { }





