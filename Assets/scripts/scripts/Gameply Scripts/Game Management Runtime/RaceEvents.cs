using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class RaceEvents
{
    protected ILeaderBoard _leaderBoard;

    public virtual int  RankPositionOfPlayer => 0; // 
    public virtual float ProgressOfPlayer => 0f;
    public virtual float RaceTimeOfPlayer => 0f;
    public virtual float DistanceCovered => 0f;
    public virtual float TimeLeft => 0f;
    public virtual float EliminationTimer => 0f;
    public virtual float TimeBeforeElimination => 0f;
    public virtual int RemainingCars => 0; 
    public virtual float TotalDistance => 0f;
    public virtual void Register(Action<string, int> action) { } 

    
    public virtual void Initialize(ILeaderBoard leaderBoard)
    {
        _leaderBoard = leaderBoard;
    }

    public abstract void RegisterResult(Action<bool> action);

    public abstract void InvokeResult(bool result);

    public abstract void Update();
    public abstract bool isRaceFinished(out bool result);

}
