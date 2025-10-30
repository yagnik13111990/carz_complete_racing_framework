using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DriftDrivableCar : MonoBehaviour
{
    KeyCode Throttle, Reverse, TurnLeft, TurnRight , SlowDownBrake , ShortBrake;

    Drift Drift;

    private Vector3 LookAheadPoint;

    ITrackUtility trackUtility;

    AIPathTraker track;

    int CurrentZoneIndex = 0;

    float steerInput;

    
    // Start is called before the first frame update
    void Start()
    {
        Drift = GetComponent<Drift>();

        track = GetComponent<AIPathTraker>();
        trackUtility = FindObjectOfType<TrackUtility>();

        Throttle = ServiceLocator.Instance.GetService<SettingManager>().M_Control.ControlSettings[ControlSettingKey.Acceleration];
        Reverse = ServiceLocator.Instance.GetService<SettingManager>().M_Control.ControlSettings[ControlSettingKey.Reverse];
        TurnLeft = ServiceLocator.Instance.GetService<SettingManager>().M_Control.ControlSettings[ControlSettingKey.SteerLeft];
        TurnRight = ServiceLocator.Instance.GetService<SettingManager>().M_Control.ControlSettings[ControlSettingKey.SteerRight];
        SlowDownBrake = ServiceLocator.Instance.GetService<SettingManager>().M_Control.ControlSettings[ControlSettingKey.ShortBrake];

        ShortBrake = KeyCode.LeftShift;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(Throttle)) Drift.SetThrottle(1f);
        else if (Input.GetKey(Reverse)) Drift.SetThrottle(-1f);
        else Drift.SetThrottle(0f);

       
        if (Input.GetKey(TurnLeft))
        {
            steerInput = Mathf.Lerp(steerInput , -1f , Time.deltaTime * 6f);          
        }
        else if (Input.GetKey(TurnRight))
        {
            steerInput = Mathf.Lerp(steerInput, 1f, Time.deltaTime * 6f);           
        }
        else
        {
            steerInput = Mathf.Lerp(steerInput, 0f, Time.deltaTime * 2f);          
        }

        Drift.SetTurn(steerInput);

        if (Input.GetKey(SlowDownBrake)) Drift.ApplyBrakeTorque(0.2f);
        else if (Input.GetKey(ShortBrake)) Drift.ApplyBrakeTorque(0.5f);
        else Drift.ReleaseBrake();

        HandleSteer();
        ReactAccordingToTrackDetails();

    }

    void HandleSteer()
    {
        LookAheadPoint = track.GetLookAheadPoint(Drift.CurrentSpeed, transform.position);

        Vector3 toLookAhead = (LookAheadPoint - transform.position).normalized;

        toLookAhead.y = 0;

       
    }


    void ReactAccordingToTrackDetails()
    {
        TrackZone CurrentZone = trackUtility.TrackZones[CurrentZoneIndex];
        TrackZone NextZone = trackUtility.TrackZones[CurrentZoneIndex + 1];


        switch (CurrentZone.type)
        {
            case TrackType.Straight:

                Drift.Acceleration = Drift.BaseAcceleration * 1f;

                if (NextZone.type == TrackType.HairpinTurn || NextZone.type == TrackType.SharpTurn)
                {
                    Drift.Acceleration = Drift.BaseAcceleration * 0.6f;
                }
                break;

            case TrackType.GentleTurn:

                Drift.Acceleration = Drift.BaseAcceleration * 0.9f;
                break;

            case TrackType.MediumTurn:

                Drift.Acceleration = Drift.BaseAcceleration * 0.75f;

                if (NextZone.type == TrackType.HairpinTurn || NextZone.type == TrackType.SharpTurn)
                {
                    Drift.Acceleration = Drift.BaseAcceleration * 0.6f;
                }
                break;


            case TrackType.SharpTurn:

                Drift.Acceleration = Drift.BaseAcceleration * 0.6f;
                break;

            default:

                Drift.Acceleration = Drift.BaseAcceleration * 0.5f;
                break;


        }

    }
}
