using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using System;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using System.Runtime.ConstrainedExecution;

public partial struct CarSystem : ISystem
{   
    public void OnUpdate(ref SystemState state)
    {
        using NativeArray<Entity> entities = SystemAPI.QueryBuilder()
        .WithAll<MovementComponent, GearComponent, GearMechenismComponent, BrakeComponent, TurnComponent, WheelComponent >()
        .Build()
        .ToEntityArray(Allocator.Temp);

        foreach (Entity entity in entities)
        {
      
            GearCalculation(ref state,entity );
            TurningCalculation(ref state , entity);         
            ApplyDownForce(ref state, entity);

        }

    }




    private void GearCalculation(ref SystemState state, Entity entity)                                                         //gearbox
    {

        RefRW<GearComponent> gear = SystemAPI.GetComponentRW<GearComponent>(entity);
        RefRO<GearMechenismComponent> gearMech = SystemAPI.GetComponentRO<GearMechenismComponent>(entity);
        RefRW<MovementComponent> movement = SystemAPI.GetComponentRW<MovementComponent>(entity);
        RefRW<BrakeComponent> brake = SystemAPI.GetComponentRW<BrakeComponent>(entity);

        // Cache gear data
        int CurrentGear = gear.ValueRO.CurrGear;

        float motorRPM = gear.ValueRO.MotorRPM;
        float maxRPM = gear.ValueRO.MaximumRPM;
        float minRPM = gear.ValueRO.MinimumRPM;
        float cutOffTimer = gear.ValueRO.CutOffTimer;
        float cutOffRPM = gear.ValueRO.CutOffRPM;
        float motorTorqueOfCurve = gear.ValueRO.MotorTorqueOfCurve;
        float lerpSpeed = gear.ValueRO.LerpSpeed;
        float engineRPM = gear.ValueRO.EngineRPM;
        float throttle = gear.ValueRO.ThrottleInput;
        float torqueLimit = movement.ValueRO.TorqueLimit;
        float currTorque = movement.ValueRW.CurrTorque;

        bool IsengineRPMInCutOffSituation = gear.ValueRW.IsEngineRPMInCutOffSituation;
        bool IsAuto = gear.ValueRW.IsAuto;


        // Cache blob data
        BlobAssetReference<GearMechenism> blob = gearMech.ValueRO.blob;
        ref BlobArray<float> gearRatios = ref blob.Value.GearRatios;

        if (IsengineRPMInCutOffSituation)
        {

            if (cutOffTimer > 0)
            {
                cutOffTimer -= Time.fixedDeltaTime;
                engineRPM = math.lerp(engineRPM, cutOffRPM, lerpSpeed * Time.fixedDeltaTime);


            }
            else
            {
                IsengineRPMInCutOffSituation = false;
            }
        }


        if (!IsengineRPMInCutOffSituation)
        {


            float targetRPM = math.abs(((motorRPM + 20f) * gearRatios[CurrentGear]));

            targetRPM = math.clamp(targetRPM, minRPM, maxRPM);
            engineRPM = math.lerp(engineRPM, targetRPM, lerpSpeed * Time.fixedDeltaTime);


        }

        if (engineRPM >= cutOffRPM)
        {

            cutOffTimer = gear.ValueRO.CutOffTimer;
        }

        if (!Mathf.Approximately(throttle, 0))
        {

            if (movement.ValueRO.Direction * throttle >= 0)
            {

                if (gear.ValueRO.MotorRPM * gearRatios[CurrentGear] >= maxRPM)
                {

                    currTorque = 0;
                }

                float MaximumWheelRPMAtCurrentGear = gearRatios[CurrentGear] * engineRPM;

         
                if (gear.ValueRO.MotorRPM <= MaximumWheelRPMAtCurrentGear)
                {

                    currTorque = throttle * gear.ValueRO.MotorTorqueOfCurve * (torqueLimit * gearRatios[CurrentGear]);

                }
                else
                {

                    currTorque = 0;
                }
            }

            else
            {

                brake.ValueRW.CurrentBrake = brake.ValueRO.MaximumBrake;
            }

        }
        else
        {

            brake.ValueRW.CurrentBrake = brake.ValueRO.MaximumBrake;
            currTorque = 0;
        }


        float prevRatio = 0;
        float newRatio = 0;

        if (engineRPM > gear.ValueRO.RPM_NextGear && CurrentGear > 0 && CurrentGear < (gearRatios.Length - 1))
        {


            prevRatio = gearRatios[CurrentGear];
            CurrentGear++;
            newRatio = gearRatios[CurrentGear];
        }
        else if (engineRPM < gear.ValueRO.RPM_PrevGear && CurrentGear > 1 && (engineRPM <= motorRPM || CurrentGear != 2))
        {


            prevRatio = gearRatios[CurrentGear];
            CurrentGear--;
            newRatio = gearRatios[CurrentGear];
        }    

        if (!Mathf.Approximately(prevRatio, 0) && !Mathf.Approximately(newRatio, 0))
        {

            engineRPM = math.lerp(engineRPM, engineRPM * (newRatio / prevRatio), lerpSpeed * Time.fixedDeltaTime);

        }    

        if (movement.ValueRO.Direction <= 0 && throttle < 0)
        {

            CurrentGear = 0;
        }
        else if (CurrentGear <= 1 && movement.ValueRO.Direction >= 0 && throttle > 0)
        {

            CurrentGear = 2;
        }
        else if (movement.ValueRO.Direction == 0 && throttle == 0)
        {

            CurrentGear = 1;
            currTorque = 0;

        }

        movement.ValueRW.CurrTorque = currTorque;

        gear.ValueRW.CurrGear = CurrentGear;

        gear.ValueRW.EngineRPM = engineRPM;

       

    }
 


    private void TurningCalculation(ref SystemState state,  Entity entity)                                                          //turn
    {

        RefRW<TurnComponent> turn = SystemAPI.GetComponentRW<TurnComponent>(entity);
        RefRW<MovementComponent> movement = SystemAPI.GetComponentRW<MovementComponent>(entity);

        float helpAngle = turn.ValueRO.HelpSteerPower;
        float steer = turn.ValueRO.TurnMultiplierInput;
        float angle = turn.ValueRO.Angle;
        float speedProcent = (movement.ValueRO.Speed / movement.ValueRO.MaximumSpeed);
        float FinalMaximumAngle = math.lerp(turn.ValueRO.MaxSteerAngle, turn.ValueRO.MaxSteerAngle * 0.3f, speedProcent);

        // angle = math.clamp(angle , -FinalMaxAngle , FinalMaxAngle);

        if (!SystemAPI.HasComponent<AICar>(entity))
        {

            turn.ValueRW.SteerAngle = math.lerp(turn.ValueRW.SteerAngle, math.clamp((steer * turn.ValueRO.MaxSteerAngle), -FinalMaximumAngle, FinalMaximumAngle), SystemAPI.Time.DeltaTime * 10f);
           
        }
        else
        {
            turn.ValueRW.SteerAngle = math.lerp(turn.ValueRW.SteerAngle, math.clamp(((steer + 1) * turn.ValueRO.Angle), -FinalMaximumAngle, FinalMaximumAngle), SystemAPI.Time.DeltaTime * 10f);
           
        }

    }



    private void ApplyDownForce(ref SystemState state , Entity entity)                                                                          //downforce
    {
        RefRW<MovementComponent> movement = SystemAPI.GetComponentRW<MovementComponent>(entity);
        float speed = movement.ValueRO.Speed;
        movement.ValueRW.DownForce = speed * 100f;
       
    }

   
}
