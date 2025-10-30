
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Splines;

public class AIDrivable : MonoBehaviour
{
   
    AIPathTraker track;
 
    IDriveHandle driveHandle;
    ITrackUtility trackUtility;
    ICommonCarEntity commonCarEntity;

    public Vector3 LookAheadPoint { get; private set; }
    public Vector3 CenterLookAhead;

    int CurrentZoneIndex = 0;

    float LaneOffset;
 
    bool ApplyLimits;
    
    void Start()
    {
        commonCarEntity = GetComponent<CommonEntity>();

        track = GetComponent<AIPathTraker>();
        trackUtility = FindObjectOfType<TrackUtility>();

        driveHandle = GetComponent<DriveHandle>();

        LaneOffset = track.LaneOffsetRange;

    }

    void Update()
    {
        IndexOfCurrentZone();
        ReactAccordingToTrackDetails();

       
    }

    private void FixedUpdate()
    {
        HandleSteer();

        driveHandle.ApplyThrottle();
        driveHandle.ApplyBrakes();

        driveHandle.ApplySteer();
    }



    void HandleSteer()
    {
     
        CenterLookAhead = track.GetLookAheadPoint(commonCarEntity.Speed, transform.position);

       
        Vector3 forward = CenterLookAhead.normalized;
        Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;

        LookAheadPoint = CenterLookAhead + right * LaneOffset;

      
        Vector3 toLookAhead = (LookAheadPoint - transform.position).normalized;
        toLookAhead.y = 0;

        float steeringAngle = Vector3.SignedAngle(transform.forward, toLookAhead, Vector3.up);

       
        float steerThreshold = 10f;
        float steerSmoothness = 5f; 

       
        if (Mathf.Abs(steeringAngle) > steerThreshold)
        {
           
            commonCarEntity.Angle = Mathf.Lerp(
                commonCarEntity.Angle,
                steeringAngle,
                Time.deltaTime * steerSmoothness
            );
        }
        else
        {
          
            commonCarEntity.Angle = Mathf.Lerp(
                commonCarEntity.Angle,
                0f,
                Time.deltaTime * (steerSmoothness * 5f)
            );
        }
    }






    void IndexOfCurrentZone()
    {
        CurrentZoneIndex = track.GetCurrentIndex();
      
        int KnotIndex = CurrentZoneIndex;

        for (int i = 0; i < trackUtility.TrackZones.Count; i++)
        {
            if (KnotIndex >= trackUtility.TrackZones[i].start_index && KnotIndex <= trackUtility.TrackZones[i].end_index)
            {
                CurrentZoneIndex = i; break;
            }
        }
    }

    void ReactAccordingToTrackDetails()
    {
        TrackZone CurrentZone = trackUtility.TrackZones[CurrentZoneIndex];
        TrackZone NextZone = trackUtility.TrackZones[CurrentZoneIndex+1];


        if (commonCarEntity.Speed > 20f) ApplyLimits = true;
        else ApplyLimits = false;
       
        switch (CurrentZone.type)
        {
            case TrackType.Straight:

                commonCarEntity.SpeedLimit = commonCarEntity.MaximumSpeed;
                commonCarEntity.TorqueLimit = commonCarEntity.MaximumTorque;
          
                commonCarEntity.Throttle = 1f;
                commonCarEntity.BrakeInput = 0f;
                

                if (NextZone.type == TrackType.HairpinTurn || NextZone.type == TrackType.SharpTurn)
                {
                    commonCarEntity.SpeedLimit = commonCarEntity.MaximumSpeed * 0.60f;
                    commonCarEntity.TorqueLimit = commonCarEntity.MaximumTorque * 0.10f;
                    if (ApplyLimits)
                    {
                        commonCarEntity.Throttle = 0.2f;
                        commonCarEntity.BrakeInput = 0.65f;
                    }
                }
                break;

            case TrackType.GentleTurn:

                commonCarEntity.SpeedLimit = commonCarEntity.MaximumSpeed * 0.6f;
                commonCarEntity.TorqueLimit = commonCarEntity.MaximumTorque * 0.56f;
                if (ApplyLimits)
                {
                    commonCarEntity.Throttle = 0.6f;
                    commonCarEntity.BrakeInput = 0.5f;
                }
                break;

            case TrackType.MediumTurn:

                commonCarEntity.SpeedLimit = commonCarEntity.MaximumSpeed * 0.5f;
                commonCarEntity.TorqueLimit = commonCarEntity.MaximumTorque * 0.4f;

                if (ApplyLimits)
                {
                    commonCarEntity.Throttle = 0.45f;
                    commonCarEntity.BrakeInput = 0.6f;
                }

                if (NextZone.type == TrackType.HairpinTurn || NextZone.type == TrackType.SharpTurn)
                {
                    commonCarEntity.SpeedLimit = commonCarEntity.MaximumSpeed * 0.20f;
                    commonCarEntity.TorqueLimit = commonCarEntity.MaximumTorque * 0.10f;

                    if (ApplyLimits)
                    {
                        commonCarEntity.Throttle = 0.2f;
                        commonCarEntity.BrakeInput = 0.7f;
                    }
                }
                break;
                

            case TrackType.SharpTurn:

                commonCarEntity.SpeedLimit = commonCarEntity.MaximumSpeed * 0.20f;
                commonCarEntity.TorqueLimit = commonCarEntity.MaximumTorque * 0.10f;

                if (ApplyLimits)
                {
                    commonCarEntity.Throttle = 0.2f;
                    commonCarEntity.BrakeInput = 0.6f;
                }
                break;

            default:

                commonCarEntity.SpeedLimit = commonCarEntity.MaximumSpeed * 0.20f;
                commonCarEntity.TorqueLimit = commonCarEntity.MaximumTorque * 0.1f;

                if (ApplyLimits)
                {
                    commonCarEntity.Throttle = 0.2f;
                    commonCarEntity.BrakeInput = 0.7f;
                }
                break;

            
        }

    }



    private void OnDrawGizmos()
    {
        if (LookAheadPoint == Vector3.zero) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(CenterLookAhead, 1f);
       
        Gizmos.DrawLine(transform.position + transform.forward , CenterLookAhead);

        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(LookAheadPoint, 1f);

        Gizmos.DrawLine(transform.position + transform.forward , LookAheadPoint);
    }

}
