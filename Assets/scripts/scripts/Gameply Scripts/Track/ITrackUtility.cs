
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine.Splines;

public interface ITrackUtility
{ 
    void Initialize();
    SplineContainer TrackCurve { get; set; }
    Spline TrackSpline { get; set; }
    List<TrackZone> TrackZones { get; }
    NativeArray<float3> SamplePoints { get; }

    float TrackLength { get; }
   
}
