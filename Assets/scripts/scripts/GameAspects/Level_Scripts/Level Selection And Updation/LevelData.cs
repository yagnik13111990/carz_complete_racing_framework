using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RaceEvent
{
    Sprint, Elimination, LapRace, OneVOne, TimeDown
}

public enum State
{
    IsCompleted, Locked, Unlocked
}
public class LevelData 
{
    public int LevelID;

    public int Rank;

    public int Rating;

    public int Reward;

    public string SceneName;

    public RaceEvent RaceEvent;

    public State State;

    public RaceType raceType;

    public float SpawnAngle;

    public int NumberOfCars;

    public int MaxLap;
}
