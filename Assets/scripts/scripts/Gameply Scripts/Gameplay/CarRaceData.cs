using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CarRaceData : MonoBehaviour  
{
    public CarRaceStats Stats { get; private set; } = new CarRaceStats();

    private void Start()
    {
       Stats.Name = gameObject.name;
        SpawnEvent.RaiseEvent(Stats);

    }
    public void UpdateStats(int lap, float raceTime, float distance , float totalDistance)
    {
        Stats.Lap = lap;
        Stats.RaceTime = raceTime;
        Stats.Distance = distance;
        Stats.TotalDistance = totalDistance;
    }


}
 