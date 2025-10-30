using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStartEndEvent
{
    public delegate void RaceStatus();

    public static event RaceStatus OnRaceStarted;

    public static event RaceStatus OnRaceEnded;


    public static void OnRaceStartedInvokation()
    {
        OnRaceStarted?.Invoke();
    }

    public static void OnRaceFinishedInvokation()
    {
        OnRaceEnded?.Invoke();
    }

    public static void ClearAll()
    {
        OnRaceStarted = null;
        OnRaceEnded = null;
    }

}
