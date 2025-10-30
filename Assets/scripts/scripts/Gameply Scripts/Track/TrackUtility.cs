
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Splines;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Jobs;


public enum TrackType : byte
{
    Straight,
    GentleTurn,
    MediumTurn,
    SharpTurn,
    HairpinTurn
}

[System.Serializable]
public class TrackZone
{
    public TrackType type;
    public float SegmentLength;
    public int start_index;
    public int end_index;
}

public class TrackUtility : MonoBehaviour , ITrackUtility
{

    public SplineContainer TrackCurve { get; set; }
    public Spline TrackSpline { get; set; }
    public List<TrackZone> TrackZones { get { return _TrackZones; } }

    private List<TrackZone> _TrackZones = new List<TrackZone>();

    private float AngleBetweenKnots;

    Vector3 knot1Direction;

    Vector3 knot2Direction;

    Scene Level;

    private NativeArray<float3> _SamplePoints;

    public NativeArray<float3> SamplePoints => _SamplePoints;

    private float _Length;
    public float TrackLength { get => _Length; }

    int Samples = 3000;

    public static List<JobHandle> ActiveHandles = new List<JobHandle>();

    public static void RegisterToListOfActiveHandles(JobHandle handle)
    {
        ActiveHandles.Add(handle);
    }

    // Start is called before the first frame update
    public void Initialize()
    {
        Level = SceneManager.GetSceneByName(ServiceLocator.Instance.GetService<LevelManager>().selectedLevel.SceneName);

        foreach(GameObject obj in Level.GetRootGameObjects())
        {
            if(obj.TryGetComponent<SplineContainer>(out var splineContainer))
            {
                TrackCurve = splineContainer;
              
            }
        }
        
        TrackSpline = TrackCurve.Spline;

        float3[] temp = new float3[Samples];

        for(int i = 0; i < Samples; i++)
        {
            float t = (float)i/ (Samples -1) ;

            Vector3 pos = TrackCurve.EvaluatePosition(t) ;

            temp[i] = new float3(pos.x , pos.y , pos.z);
        }

        _SamplePoints = new NativeArray<float3>(temp , Allocator.Persistent);

        _Length = TrackCurve.CalculateLength();

        SetUpZones();

        
    }
                                                                                
    public (BezierKnot, BezierKnot, BezierKnot) GetKnots(int i)
    {
        if (i == 0) return (TrackSpline[TrackSpline.Count - 1], TrackSpline[(i) % TrackSpline.Count], TrackSpline[(i + 1) % TrackSpline.Count]);
        return (TrackSpline[(i - 1) % TrackSpline.Count], TrackSpline[(i) % TrackSpline.Count], TrackSpline[(i + 1) % TrackSpline.Count]);
    }

    private void SetUpZones()
    {
        for (int i = 0; i < TrackSpline.Count; i++)
        {
            (BezierKnot, BezierKnot, BezierKnot) Knots = GetKnots(i);

            float Distance = Vector3.Distance(Knots.Item2.Position, Knots.Item3.Position);

            knot1Direction = (Knots.Item2.Position - Knots.Item1.Position);
            knot2Direction = (Knots.Item3.Position - Knots.Item2.Position);

            AngleBetweenKnots = Vector3.Angle(knot1Direction, knot2Direction);

            TrackType trackType = GetTypeOfTrack((AngleBetweenKnots));

            TrackZone zone = new TrackZone()
            {
                type = trackType,
                SegmentLength = Distance,
                start_index = i,
                end_index = i + 1
            };

            _TrackZones.Add(zone);

        }
    }

    private TrackType GetTypeOfTrack(float Angle)
    {

        return
            Angle switch
            {
                <= 10f => TrackType.Straight,
                <= 25f => TrackType.GentleTurn,
                <= 50 => TrackType.MediumTurn,
                <= 70f => TrackType.SharpTurn,
                _ => TrackType.HairpinTurn
            };

    }

    private void OnDestroy()
    {
        TrackJobRegistery.CompleteAllAtOnce();
       
        if (_SamplePoints.IsCreated)
        {
           _SamplePoints.Dispose();
        }
        
    }
    
}
