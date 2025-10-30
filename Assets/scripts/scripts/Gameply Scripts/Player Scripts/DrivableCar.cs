using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;


public class DrivableCar : MonoBehaviour
{
    private float AccelerationVal, SteerVal, BrakeVal;

    IDriveHandle _driveHandle;
    ICommonCarEntity commonCarEntity;

    EntityManager entityManager;

    KeyCode Acc;
    KeyCode SteerLeft;
    KeyCode SteerRight;
    KeyCode Brake;
    KeyCode Reverse;

  
    void Start()
    {
        _driveHandle = GetComponent<DriveHandle>();

        commonCarEntity = GetComponent<CommonEntity>();

        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        entityManager.AddComponent<PlayerCar>(commonCarEntity.CarEntity);

       

        Acc = ServiceLocator.Instance.GetService<SettingManager>().M_Control.ControlSettings[ControlSettingKey.Acceleration];
        SteerLeft = ServiceLocator.Instance.GetService<SettingManager>().M_Control.ControlSettings[ControlSettingKey.SteerLeft];
        SteerRight = ServiceLocator.Instance.GetService<SettingManager>().M_Control.ControlSettings[ControlSettingKey.SteerRight];
        Brake = ServiceLocator.Instance.GetService<SettingManager>().M_Control.ControlSettings[ControlSettingKey.ShortBrake];
        Reverse = ServiceLocator.Instance.GetService<SettingManager>().M_Control.ControlSettings[ControlSettingKey.Reverse];  

    }

   
    void Update() 
    {
        if (commonCarEntity == null) return;

        AccelerationVal = 0f;
        BrakeVal = 0f;
        SteerVal = 0f;



        if (Input.GetKey(Acc)) AccelerationVal = 1f;
        if (Input.GetKey(Reverse)) AccelerationVal = -1f;
        if (Input.GetKey(Brake)) BrakeVal = 1f;

        if (Input.GetKey(SteerLeft)) SteerVal = -1f;
        if (Input.GetKey(SteerRight)) SteerVal = 1f;

        commonCarEntity.Throttle = AccelerationVal;

        commonCarEntity.TurnMultiplier = SteerVal;

        commonCarEntity.BrakeInput = BrakeVal;  

       
        _driveHandle.ApplyThrottle();
        _driveHandle.ApplySteer();
        _driveHandle.ApplyBrakes();
            
    }

  
}
