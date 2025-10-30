using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;



public class AIPathTraker : MonoBehaviour
{

    public float BaseLookAheadPointDistance;

    public float LaneOffsetRange;

    float LookAheadDistance;

    float factor = 0.1f;

    float CurrentT =0f;

    float LookAheadT;

    float SegmentT;

    float LaneOffset;

    int SegmentCount;

    Vector3 LookAheadPoint;

    ITrackUtility trackUtility;

    MidRaceCalculations MidRaceCalculations;





    // Start is called before the first frame update
    void Start()
    {
           
        trackUtility = FindObjectOfType<TrackUtility>();
 
        MidRaceCalculations = GetComponent<MidRaceCalculations>();
    
    }

    

    
    public Vector3 GetLookAheadPoint(float speed , Vector3 CarPosition)
    {
        CurrentT = MidRaceCalculations.NearestPoint;
       
        LookAheadDistance = BaseLookAheadPointDistance + speed * factor;

        LookAheadT = CurrentT + (LookAheadDistance / trackUtility.TrackLength);

        LookAheadT =  Mathf.Clamp01(LookAheadT);

        LookAheadPoint = trackUtility.TrackCurve.EvaluatePosition(LookAheadT);

      
        return LookAheadPoint;
    }

    public int GetCurrentIndex()
    {
      
        SegmentCount = trackUtility.TrackSpline.Count - 1;

        SegmentT = (float)1 / SegmentCount;
       
        int CurrentIndex = Mathf.FloorToInt(CurrentT / SegmentT);


      
        return CurrentIndex;
    }

   
  
}
