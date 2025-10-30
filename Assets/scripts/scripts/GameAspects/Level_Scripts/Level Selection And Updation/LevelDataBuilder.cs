using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDataBuilder 
{
   private LevelData LevelData = new LevelData();

    public LevelDataBuilder ID(int id)
    {
        LevelData.LevelID = id;
        return this;
    }

    public LevelDataBuilder Rank(int rank)
    {
        LevelData.Rank = rank;
        return this;
    }

    public LevelDataBuilder StarCount(int stars)
    {
        LevelData.Rating = stars;
        return this;
    }

    public LevelDataBuilder SceneName(string name)
    {
        LevelData.SceneName = name;
        return this;
    }

    public LevelDataBuilder LevelState(State state)
    {
        LevelData.State = state;
        return this;
    }

    public LevelDataBuilder RaceEvent(RaceEvent raceEvent)
    {
        LevelData.RaceEvent = raceEvent;
        return this;    
    }

    public LevelDataBuilder RaceType(RaceType type)
    {
        LevelData.raceType = type;
        return this;
    }
    public LevelDataBuilder RewardAmount(int amount)
    {
        LevelData.Reward = amount;
        return this;
    }

    public LevelDataBuilder SpawnAngleForCars(float angle)
    {
        LevelData.SpawnAngle = angle;
        return this;
    }

    public LevelDataBuilder NumberOfCars(int num)
    {
        LevelData.NumberOfCars = num;
        return this;
    }

    public LevelDataBuilder MaximumLaps(int val)
    {
        LevelData.MaxLap = val;
        return this;
    }
    public LevelData Build()
    {
        return LevelData;
    }
}
