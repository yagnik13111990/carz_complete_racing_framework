using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DriveHandle : MonoBehaviour , IDriveHandle
{
    public List<WheelCollider> wheelColliders;
    public List<Transform> Wheels;

    public Transform COM;

    [SerializeField] private ParticleSystem[] Smokes;

    private Vector3 pos;
    private Quaternion rot;
   
    Geardata gear;

    public Rigidbody RB { get; set; }

    [HideInInspector]
    public float MotorTorque {  get; set; }

    [HideInInspector]
    public float SteerAngle { get; set; }

    [HideInInspector]
    public float BrakeTorque { get; set; }

    [HideInInspector]
    public float DownForce { get; set; }

    [HideInInspector]
    public float DriftFactor {  get; set; }

    [HideInInspector]
    public float BrakeInput {  get; set; }

    [HideInInspector]
    public float MotorRPM { get; set; }

    private List<WheelFrictionCurve> _forwardFrictions = new List<WheelFrictionCurve>();
    public List<WheelFrictionCurve> ForwardFrictions { get => _forwardFrictions ; set => _forwardFrictions = value; }

    private List<WheelFrictionCurve> _sidewayFrictions = new List<WheelFrictionCurve>();
    public List<WheelFrictionCurve> SidewayFrictions { get => _sidewayFrictions; set => _sidewayFrictions = value; }

    MidRaceCalculations midRaceCalculations;

    bool ShouldSmoke;


    void Start()
    {
        RB = GetComponent<Rigidbody>();

        StartCoroutine(EnableInterpolationNextFixedFrame());

        RB.centerOfMass = COM.localPosition;

        midRaceCalculations = GetComponent<MidRaceCalculations>();

        
        for(int i = 0; i < wheelColliders.Count; i++) 
        {
            _forwardFrictions.Add(wheelColliders[i].forwardFriction);
            _sidewayFrictions.Add(wheelColliders[i].sidewaysFriction);
        }

        midRaceCalculations.OnReachingFinishLine += StopCarOnRaceFinished;

        GameStartEndEvent.OnRaceEnded += StopCarOnRaceFinished;


    }
   
    IEnumerator EnableInterpolationNextFixedFrame()
    {
        yield return new WaitForFixedUpdate(); // wait until physics runs once
        RB.interpolation = RigidbodyInterpolation.Interpolate;
    }
    private void FixedUpdate()
    {
        ApplyDownForce();

        CheckSlipForSmoke();

        
       
    }

    private void Update()
    {
        VisualChangesInWheels();
       
        MotorRPM = wheelColliders[0].rpm;

        ApplyFrictions();
    }

    IEnumerator ApplyBrakesOnRaceEnd()
    {

        foreach (WheelCollider wc in wheelColliders)
        {
            wc.motorTorque = 0f;
            wc.brakeTorque = 7000f;
        }


        yield return new WaitForSeconds(3f);

        Destroy(this.gameObject);
    }

    void StopCarOnRaceFinished()
    {
        StartCoroutine(ApplyBrakesOnRaceEnd());
    }

    public void ApplyFrictions()
    {
        for(int i = 0; i < wheelColliders.Count; i++)
        {
            wheelColliders[i].forwardFriction = _forwardFrictions[i];
            wheelColliders[i].sidewaysFriction = _sidewayFrictions[i];
        }
    }

    void CheckDriftFactor()
    {
        for (int i = 2; i < 4; i++)
        {
            if (wheelColliders[i].GetGroundHit(out WheelHit hit))
            {
                if (hit.sidewaysSlip < 0)
                    DriftFactor = (1 - Mathf.Clamp01(SteerAngle)) * Mathf.Abs(hit.sidewaysSlip);

                else if (hit.sidewaysSlip > 0)
                    DriftFactor = (1 + Mathf.Clamp01(SteerAngle)) * Mathf.Abs(hit.sidewaysSlip);
            }
        }
    }





    public void ApplyBrakes() 
    {
      
        foreach (WheelCollider wc in wheelColliders)
        {
            wc.brakeTorque = BrakeTorque * BrakeInput;
           
        }
    }



    public void ApplyDownForce()
    {     
        RB.AddForce(-transform.up * DownForce , ForceMode.Force);       
    }
    public void VisualChangesInWheels()
    {
        for (int i = 0; i < wheelColliders.Count; i++)
        {
            UpdateWheelPosRot(Wheels[i], wheelColliders[i]);
        }
    }

    public void UpdateWheelPosRot(Transform wheel, WheelCollider col)
    {
        pos = wheel.position;
        rot = wheel.rotation;

        col.GetWorldPose(out pos, out rot);

        wheel.position = pos;
        wheel.rotation = rot;
    }

    void CheckSlipForSmoke()
    {
        WheelHit hit;
        for(int i = 2; i < wheelColliders.Count; i++)
        {
            if (wheelColliders[i].GetGroundHit(out hit))
            {
                if(hit.sidewaysSlip >= 0.2f || hit.sidewaysSlip <= -0.2f || hit.forwardSlip >= 0.2f || hit.forwardSlip <= -0.2f) ShouldSmoke = true;

                else ShouldSmoke = false;
            }
        }

        if (ShouldSmoke) StartSmoke();
        else StopSmoke();
    }

    void StartSmoke()
    {
        for(int i = 0;  i <Smokes.Length; i++)
        {
            Smokes[i].Play();
        }
    }

    void StopSmoke()
    {
        for (int i = 0; i < Smokes.Length; i++)
        {
            Smokes[i].Stop();
        }
    }

    public void ApplyThrottle() {

        wheelColliders[0].motorTorque = MotorTorque;
        wheelColliders[1].motorTorque = MotorTorque; 
        wheelColliders[2].motorTorque = MotorTorque;
        wheelColliders[3].motorTorque = MotorTorque;
   
    }
    public void ApplySteer () 
    {
        wheelColliders[0].steerAngle = SteerAngle;
        wheelColliders[1].steerAngle = SteerAngle;
       
    }




    private void OnDestroy()
    {
        midRaceCalculations.OnReachingFinishLine -= StopCarOnRaceFinished;

        GameStartEndEvent.OnRaceEnded -= StopCarOnRaceFinished;
    }

}
    

