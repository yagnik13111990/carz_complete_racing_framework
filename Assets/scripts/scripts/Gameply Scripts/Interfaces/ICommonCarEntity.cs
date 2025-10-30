
using Unity.Entities;
using UnityEngine;

public interface ICommonCarEntity
{
    float MaximumSpeed { get; set; }
    float MaximumTorque { get; set; }
    float SpeedLimit { get; set; }
    float Speed { get; set; }
    int Gear { get; set; }
    float BrakeInput { get; set; }
    float Throttle { get; set; }
    float TurnMultiplier { get; set; }
    float Angle { get; set; }
    float MaximumSpeedDamper { get; set; }
    float TorqueLimit { get; set; }

    Entity CarEntity {  get; set; }


    void InitializeDamper(float damper);

}
   
