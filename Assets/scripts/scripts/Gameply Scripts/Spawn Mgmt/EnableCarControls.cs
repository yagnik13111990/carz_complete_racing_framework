using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableCarControls : MonoBehaviour
{
   
    GameplayManager gameplayManager;

    MidRaceCalculations midRaceCalculations;

    bool isEnabled;
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
        if (this.gameObject.TryGetComponent<AIEntity>(out AIEntity aiEntity)) aiEntity.enabled = true;
        if (this.gameObject.TryGetComponent<AIDrivable>(out AIDrivable aiDrivable)) aiDrivable.enabled = true;
        if (this.gameObject.TryGetComponent<AIPathTraker>(out AIPathTraker aiPathTracker)) aiPathTracker.enabled = true;
        if (this.gameObject.TryGetComponent<DrivableCar>(out DrivableCar drivableCar)) drivableCar.enabled = true;
        if (this.gameObject.TryGetComponent<CommonEntity>(out CommonEntity commonEntity)) commonEntity.enabled = true;
        if (this.gameObject.TryGetComponent<ControlHelper>(out ControlHelper controlHelper)) controlHelper.enabled = true;
       

    }

    void DisableControl()
    {
        if (this.gameObject.TryGetComponent<AIEntity>(out AIEntity aiEntity)) aiEntity.enabled = false;
        if (this.gameObject.TryGetComponent<AIDrivable>(out AIDrivable aiDrivable)) aiDrivable.enabled = false;
        if (this.gameObject.TryGetComponent<AIPathTraker>(out AIPathTraker aiPathTracker)) aiPathTracker.enabled = false;
        if (this.gameObject.TryGetComponent<DrivableCar>(out DrivableCar drivableCar)) drivableCar.enabled = false;
        if (this.gameObject.TryGetComponent<CommonEntity>(out CommonEntity commonEntity)) commonEntity.enabled = false;
        if (this.gameObject.TryGetComponent<ControlHelper>(out ControlHelper controlHelper)) controlHelper.enabled = false;
       


    }


    private void OnDestroy()
    {
        GameStartEndEvent.OnRaceStarted -= EnableControl;
        GameStartEndEvent.OnRaceEnded -= DisableControl;
    }
}
