using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpawnEvent
{
    public static event Action<CarRaceStats> OnSpawnEvent;

    public static void RaiseEvent(CarRaceStats car)
    {
        OnSpawnEvent?.Invoke(car);
    }
}
