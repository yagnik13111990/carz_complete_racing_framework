using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimeTrial : RaceEvents
{
    private float _TimeLeft;
    public override float TimeLeft => _TimeLeft;

    private int _RankPositionOfPlayer;
    public override int RankPositionOfPlayer => _RankPositionOfPlayer;


    CarRaceStats Player;

    public event Action<bool> RaceResult;

    public override void Initialize(ILeaderBoard leaderBoard)
    {
        base.Initialize(leaderBoard);

        Player = _leaderBoard.CarsInRace.FirstOrDefault(a => a.Name == "You") ;

        _TimeLeft = 60f;

    }

    public override void Update()
    {
        if (_leaderBoard == null || _leaderBoard.CarsInRace.Count == 0)
            return;

        _TimeLeft -= Time.deltaTime;

        _leaderBoard.SortTheList();

        _RankPositionOfPlayer = _leaderBoard.CarsInRace.IndexOf(Player) + 1;

        
            
    }

    public override bool isRaceFinished(out bool result)
    {
       if(_TimeLeft <= 0)
       {
            _TimeLeft = 0;

            if(_RankPositionOfPlayer == 1) result = true;

            else result = false;

            return true;

        }

        result = false;

        return false;
    }

    public override void RegisterResult(Action<bool> action)
    {
        RaceResult -= action;

        RaceResult += action;
    }

    public override void InvokeResult(bool result)
    {
        RaceResult?.Invoke(result);
    }
}
