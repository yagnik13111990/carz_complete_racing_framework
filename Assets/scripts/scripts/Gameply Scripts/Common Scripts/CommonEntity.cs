using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Profiling;


public class CommonEntity : MonoBehaviour, ICommonCarEntity
{
    #region Global Properties
    public float MaximumSpeed { get; set; }
    public float MaximumTorque { get; set; }
    public float SpeedLimit { get; set; }
    public float Speed { get; set; }
    public int Gear { get; set; }
    public float BrakeInput { get; set; }
    public float Throttle { get; set; }
    public float TurnMultiplier { get; set; }
    public float MaximumSpeedDamper {  get; set; }
    public float Angle { get; set; }
    public float TorqueLimit { get; set; }

    #endregion

    #region Variables

    [SerializeField] private MovementData movementData;
    [SerializeField] private TurnData turnData;
    [SerializeField] private BrakeData brakeData;
    [SerializeField] private Geardata geardata;


    public float[] Ratios = { 3.2f, 2.1f, 1.5f, 1.15f, 0.9f };
    public float[] GearsRatios;

    private IDriveHandle driveHandle;
    DriftEntity de;

    public AnimationCurve MotorTorqueCurve;


    private EntityManager entityManager;
    private BlobAssetReference<GearMechenism> _blob;

    TurnComponent turn;
    BrakeComponent brake;
    MovementComponent movement;

    GearComponent gear;

    public Entity CarEntity { get; set; }
    // Entity DriftEntity;

    #endregion
    // Start is called before the first frame update
    void Awake()
    {
        driveHandle = GetComponent<DriveHandle>();
        de = GetComponent<DriftEntity>();


        BlobBuilder builder = new BlobBuilder(Allocator.Temp);
        ref GearMechenism gear_mech = ref builder.ConstructRoot<GearMechenism>();

        BlobBuilderArray<float> blobArray_Ratios = builder.Allocate(ref gear_mech.GearRatios, Ratios.Length);
        BlobBuilderArray<float> blobArray_GearRatios = builder.Allocate(ref gear_mech.GearRatios, Ratios.Length + 2);


        for (int count = 0; count < blobArray_Ratios.Length; count++)
        {
            blobArray_Ratios[count] = Ratios[count];
        }

        blobArray_GearRatios[0] = 4;  //reverse gear
        blobArray_GearRatios[1] = 0;  //neutral gear

        for (int count = 2; count < blobArray_GearRatios.Length; count++)
        {
            blobArray_GearRatios[count] = Ratios[count - 2] * 3.65f;
        }



        _blob = builder.CreateBlobAssetReference<GearMechenism>(Allocator.Persistent);
        builder.Dispose();

        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        #region Entity And Component SetUp

        EntityArchetype blueprint = entityManager.CreateArchetype
                (
                  typeof(MovementComponent),
                   typeof(TurnComponent),
                   typeof(BrakeComponent),
                   typeof(WheelComponent),
                   typeof(GearComponent),
                   typeof(GearMechenismComponent)
                );

        CarEntity = entityManager.CreateEntity(blueprint);

        entityManager.AddComponentData(CarEntity, new GearMechenismComponent { blob = _blob });

      
      
       


        entityManager.AddComponentData(CarEntity, new MovementComponent
        {
            Speed = 0,
            MaximumSpeed = movementData.MaximumSpeed, //160
            SpeedLimit = movementData.MaximumSpeed,
            DownForce = 0,
            MaximumTorque = movementData.MaximumTorque,
            CurrTorque = 0,
            Velocity = 0,
            Direction = 0,

        });



        entityManager.AddComponentData(CarEntity, new TurnComponent
        {
            MaxSteerAngle = turnData.MaxSteerAngle,
            AngleLimitAtSpeed = turnData.AngleLimitAtSpeed,
            SteerAngle = 0,
            DriftHelper = 0,
            TurnMultiplierInput = turnData.TurnMultiplier,
            Angle = 60f
        });

        entityManager.AddComponentData(CarEntity, new GearComponent
        {
            CurrGear = 1,
            MaximumRPM = geardata.MaximumRPM,
            MinimumRPM = geardata.MinimumRPM,
            MotorRPM = geardata.MotorRPM,
            CutOffRPM = geardata.CutOffRPM,
            CutOffTimer = geardata.CutOffTimer,
            LerpSpeed = geardata.LerpSpeed,
            MotorTorqueOfCurve = 0,
            EngineRPM = 0,
            IsEngineRPMInCutOffSituation = false,
            RPM_NextGear = geardata.RPM_NextGear,
            RPM_PrevGear = geardata.RPM_PrevGear,
            MaxForwardSlipToAvoidChangeGear = geardata.MaxForwardSlipToAvoidChangeGear,
            ThrottleInput = geardata.Throttle,
            IsAuto = true
        });

        entityManager.AddComponentData(CarEntity, new BrakeComponent
        {
            MaximumBrake = brakeData.MaximumBrake,
            CurrentBrake = 0,
            BrakeInput = brakeData.BrakeInput
        });




        #endregion

        SpeedLimit = movementData.MaximumSpeed;
        MaximumSpeed = movementData.MaximumSpeed;
        MaximumTorque = movementData.MaximumTorque;
        TorqueLimit = movementData.MaximumTorque;
    }

    public void InitializeDamper(float damper)
    {
    //    MaximumSpeedDamper = damper;
    //    movementData.MaximumSpeed *= MaximumSpeedDamper;
    //    movementData.MaximumTorque *= MaximumSpeedDamper;
    }

    void Update()
    {

        movement = entityManager.GetComponentData<MovementComponent>(CarEntity);
        turn = entityManager.GetComponentData<TurnComponent>(CarEntity);
        brake = entityManager.GetComponentData<BrakeComponent>(CarEntity);
        gear = entityManager.GetComponentData<GearComponent>(CarEntity);

        SystemVariableSetUp();

        ApplyToCar();

        movement.SpeedLimit = SpeedLimit;
        movement.TorqueLimit = TorqueLimit;
        entityManager.SetComponentData<MovementComponent>(CarEntity, movement);

        brake.BrakeInput = BrakeInput;
        entityManager.SetComponentData<BrakeComponent>(CarEntity, brake);

        gear.ThrottleInput = Throttle;

        entityManager.SetComponentData<GearComponent>(CarEntity, gear);

        turn.TurnMultiplierInput = TurnMultiplier;

        turn.Angle = Angle;

        entityManager.SetComponentData<TurnComponent>(CarEntity, turn);

        WriteGlobalProperties();

    }

    private void FixedUpdate()
    {
       
        LimitTheSpeed();
    }
    #region Methods 

    private void WriteGlobalProperties()
    {
        Speed = movement.Speed;
        Gear = geardata.CurrGear;
    }

    private void ApplyToCar()
    {
        if (driveHandle != null)
        {

            driveHandle.MotorTorque = movement.CurrTorque;
            driveHandle.SteerAngle = turn.SteerAngle;
            driveHandle.BrakeTorque = brake.CurrentBrake;
            driveHandle.BrakeInput = brake.BrakeInput;
            driveHandle.DownForce = movement.DownForce;
            
            gear.MotorRPM = driveHandle.MotorRPM;
        }
    }

    private void SystemVariableSetUp()
    {

        movement.Speed = driveHandle.RB.velocity.magnitude;
        movement.Speed *= 3.6f;

        if (movement.Speed < 1f) movement.Speed = 0f;

        movement.Direction = movementData.Speed < 1f ? 0 : Vector3.Dot(transform.forward, driveHandle.RB.velocity.normalized) >= 0 ? 1 : -1;

        gear.MotorTorqueOfCurve = MotorTorqueCurve.Evaluate(gear.EngineRPM * 0.001f);

    }

    private void LimitTheSpeed()
    {
       
        if (movement.Speed >= SpeedLimit)
        {

            
            driveHandle.RB.velocity = driveHandle.RB.velocity.normalized * (SpeedLimit / 3.6f);
        }
    }

    #endregion
}




#region Data SetUp
[System.Serializable]
public struct MovementData
{
    public float MaximumSpeed;
    public float MaximumTorque;
    public float Speed;
    public float SpeedLimit;
    public float DownForce;


}



[System.Serializable]
public struct Geardata
{

    public int CurrGear;
    public float MaximumRPM;
    public float MinimumRPM;
    public float MotorRPM;
    public float CutOffTimer;
    public float CutOffRPM;
    public float MotorTorqueOfCurve;
    public float LerpSpeed;
    public float RPM_NextGear;
    public float RPM_PrevGear;
    public float MaxForwardSlipToAvoidChangeGear;
    public float Throttle;
}

[System.Serializable]
public struct TurnData
{
    public float MaxSteerAngle;
    public float AngleLimitAtSpeed;
    public float TurnMultiplier;
    public float Angle;
    public float SteerAngle;
}

[System.Serializable]
public struct BrakeData
{
    public float MaximumBrake;
    public float BrakeInput;
}


#endregion