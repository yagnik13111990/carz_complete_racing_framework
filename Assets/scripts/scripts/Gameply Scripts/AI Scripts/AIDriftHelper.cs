using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDriftHelper : MonoBehaviour
{
  
    private Vector3 LookAheadPoint;

    ITrackUtility trackUtility;

    AIPathTraker track;

    int CurrentZoneIndex = 0;

    Drift Drift;
    void Start()
    {
        track = GetComponent<AIPathTraker>();
        trackUtility = FindObjectOfType<TrackUtility>();

        Drift = GetComponent<Drift>();
    }

   
    void Update()
    {
        HandleSteer();

        HandleThrottle();

        ReactAccordingToTrackDetails();
    }


    void HandleSteer()
    {

        LookAheadPoint = track.GetLookAheadPoint(Drift.CurrentSpeed, transform.position);
           

        Vector3 toLookAhead = (LookAheadPoint - transform.position);
        toLookAhead.y = 0f;

      
        float rawAngle = Vector3.SignedAngle(transform.forward, toLookAhead.normalized, Vector3.up);

        
        float turnInput = Mathf.Clamp(rawAngle / 30f, -1f, 1f);

        Drift.SetTurn(turnInput);
    }

    void HandleThrottle()
    {
        
        float distanceToTarget = (LookAheadPoint - transform.position).magnitude;

        
        float throttleInput = 1f;

        if (distanceToTarget < 5f)
            throttleInput = 0.6f; 

        Drift.SetThrottle(throttleInput);
    }

    void ReactAccordingToTrackDetails()
    {
        TrackZone CurrentZone = trackUtility.TrackZones[CurrentZoneIndex];
        TrackZone NextZone = trackUtility.TrackZones[CurrentZoneIndex + 1];


        switch (CurrentZone.type)
        {
            case TrackType.Straight:

                Drift.Acceleration = Drift.BaseAcceleration * 1f;
                Drift.ReleaseBrake();

                if (NextZone.type == TrackType.HairpinTurn || NextZone.type == TrackType.SharpTurn)
                {
                    Drift.Acceleration = Drift.BaseAcceleration * 0.6f;
                    Drift.ApplyBrakeTorque(0.1f);
                }
                break;

            case TrackType.GentleTurn:

                Drift.Acceleration = Drift.BaseAcceleration * 0.9f;
                Drift.ApplyBrakeTorque(0.2f);
                break;

            case TrackType.MediumTurn:

                Drift.Acceleration = Drift.BaseAcceleration * 0.75f;

                if (NextZone.type == TrackType.HairpinTurn || NextZone.type == TrackType.SharpTurn)
                {
                    Drift.Acceleration = Drift.BaseAcceleration * 0.4f;
                    Drift.ApplyBrakeTorque(0.1f);
                }
                break;


            case TrackType.SharpTurn:

                Drift.Acceleration = Drift.BaseAcceleration * 0.5f;
                Drift.ApplyBrakeTorque(0.1f);
                break;

            default:

                Drift.Acceleration = Drift.BaseAcceleration * 0.4f;
                Drift.ApplyBrakeTorque(0.1f);
                break;


        }


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(LookAheadPoint, 2f);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position + transform.forward * 3.86f, LookAheadPoint);
    }
}
