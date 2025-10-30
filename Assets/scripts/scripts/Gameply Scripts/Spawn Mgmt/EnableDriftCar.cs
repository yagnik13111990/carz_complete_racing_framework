using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDriftCar : MonoBehaviour
{
    GameplayManager gameplayManager;

    MidRaceCalculations midRaceCalculations;

    private void Awake()
    {
        gameplayManager = FindFirstObjectByType<GameplayManager>();
        midRaceCalculations = GetComponent<MidRaceCalculations>();

    }
    void Start()
    {

        if (gameplayManager == null) return;

        GameStartEndEvent.OnRaceStarted += EnableControl;
        GameStartEndEvent.OnRaceEnded += DisableControl;

        midRaceCalculations.OnReachingFinishLine += DisableControl;
    }

    void EnableControl()
    {    
        if (this.gameObject.TryGetComponent<AIPathTraker>(out AIPathTraker aiPathTracker)) aiPathTracker.enabled = true;
        if(this.gameObject.TryGetComponent<DriftDrivableCar>(out DriftDrivableCar driftDrivableCar)) driftDrivableCar.enabled = true;
        if(this.gameObject.TryGetComponent<AIDriftHelper>(out AIDriftHelper aiDriftHelper)) aiDriftHelper.enabled = true;
     

    }

    void DisableControl()
    {
        if (this.gameObject.TryGetComponent<AIPathTraker>(out AIPathTraker aiPathTracker)) aiPathTracker.enabled = false;
        if (this.gameObject.TryGetComponent<DriftDrivableCar>(out DriftDrivableCar driftDrivableCar)) driftDrivableCar.enabled = false;
        if (this.gameObject.TryGetComponent<AIDriftHelper>(out AIDriftHelper aiDriftHelper)) aiDriftHelper.enabled = false;
       
    }


    private void OnDestroy()
    {
        GameStartEndEvent.OnRaceStarted -= EnableControl;
        GameStartEndEvent.OnRaceEnded -= DisableControl;
    }
}
