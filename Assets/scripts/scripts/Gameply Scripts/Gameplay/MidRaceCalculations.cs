using System;
using System.Threading;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;


public class MidRaceCalculations : MonoBehaviour {

    private CarRaceData car;

    private ITrackUtility trackUtility;

    private int Lap = 1;

    private int MaxLap;

    private float RaceTime = 0;

    private float DistanceTravelledSoFar = 0f;
   
    private float TotalLenghtOfTrack = 0;

    private float _NearestPoint;

    private float LastT;
    public float NearestPoint => _NearestPoint;

    private float LapThresh = 0.05f;

    private bool isTiming;

    GameplayManager gameplayManager;

    private FindNearestPointJob findNearestPointJob;

    private JobHandle handle;

    private NativeArray<float> jobResult;

    public event Action OnReachingFinishLine;

    // Start is called before the first frame update
    void Start()
    {
        car = GetComponent<CarRaceData>();

        trackUtility = FindObjectOfType<TrackUtility>();

        gameplayManager = FindFirstObjectByType<GameplayManager>();

        if (trackUtility == null) return;

        MaxLap = ServiceLocator.Instance.GetService<LevelManager>().selectedLevel.MaxLap;

        TotalLenghtOfTrack = trackUtility.TrackSpline.GetLength();

        TotalLenghtOfTrack *=  MaxLap;


        jobResult = new NativeArray<float>(1, Allocator.Persistent);

        findNearestPointJob = new FindNearestPointJob
        {
            TrackPathPoints = trackUtility.SamplePoints,
            ResultT = jobResult
        };


        GameStartEndEvent.OnRaceStarted += RaceStarted;
        GameStartEndEvent.OnRaceEnded += RaceFinished;

        OnReachingFinishLine += RaceFinished;

       
        
    }

    // Update is called once per frame
    void Update()
    {
        if (trackUtility == null) return;

       
        CalculateTime();

        GetNearestPointToCar(transform.position);

        CalculateDistanceAndLap(_NearestPoint);

        car.UpdateStats(Lap, RaceTime , DistanceTravelledSoFar , TotalLenghtOfTrack);


        OnReachingFinishLineInvoker();
        
    }
   
 
    public void GetNearestPointToCar(Vector3 CarPos)
    {

        handle.Complete();

        _NearestPoint = jobResult[0];

        // Update job with current car position
        findNearestPointJob.CarPosition = transform.position;

        // Schedule new job
        handle = findNearestPointJob.Schedule();

        TrackJobRegistery.AddToRgisterOfHandle(handle);

    }

    private void CalculateDistanceAndLap(float T)
    {
       

        if (LastT > 1f - LapThresh && T < LapThresh) { Lap++;  }


        float difference = T - LastT;

        if(Mathf.Abs(difference) > 0.5f) difference = 0f;

        if (difference > 0f) DistanceTravelledSoFar += (difference * TotalLenghtOfTrack) / MaxLap;

        LastT = T;
    }
   
    void CalculateTime()
    {
        if (isTiming)
        {
            RaceTime += Time.deltaTime;
        }
    }

    void RaceStarted()
    {
        isTiming = true;

        GameStartEndEvent.OnRaceStarted -= RaceStarted;
    }

    void RaceFinished()
    {
        isTiming = false;
       
        GameStartEndEvent.OnRaceStarted -= RaceFinished;
        OnReachingFinishLine -= RaceFinished;

    }

  
    void OnReachingFinishLineInvoker()
    {
        if((ServiceLocator.Instance.GetService<LevelManager>().selectedLevel.RaceEvent is not (RaceEvent.Elimination or RaceEvent.TimeDown)) && DistanceTravelledSoFar >= TotalLenghtOfTrack)
        {
            OnReachingFinishLine?.Invoke();
        }
    }

    private void OnDestroy()
    {
        
        handle.Complete();

        if (jobResult.IsCreated) jobResult.Dispose();
    }
}
