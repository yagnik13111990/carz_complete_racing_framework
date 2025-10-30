using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LapRace : RaceEvents
{
   
    public int _RankPositionOfPlayer { get; private set; }
    public override int RankPositionOfPlayer => _RankPositionOfPlayer;

    public float _Timer {  get; private set; }
    public override float RaceTimeOfPlayer => _Timer;


    public event Action<string, int> OnLapChange;

    public int PreviousLap;

    public CarRaceStats Player;

    public event Action<bool> RaceResult;



    public override void Register(Action<string, int> action)
    {
        OnLapChange += action;
    }

    public override void Initialize(ILeaderBoard leaderBoard)
    {
        base.Initialize(leaderBoard);

        Player = _leaderBoard.CarsInRace.FirstOrDefault(a => a.Name == "You");
        PreviousLap = 1;
    }

    public override void Update()
    {
        if (_leaderBoard == null || _leaderBoard.CarsInRace.Count == 0)
            return;

        _leaderBoard.SortTheList();

        _RankPositionOfPlayer = _leaderBoard.CarsInRace.IndexOf(Player) + 1;

        _Timer = Player.RaceTime;

        int CurrentLap = Player.Lap;

        if(PreviousLap != CurrentLap )
        {
            PreviousLap = Player.Lap;

            OnLapChange?.Invoke($"LAP {Player.Lap - 1  } COMPLETED " , Player.Lap );
        }

      
    }

    public override bool isRaceFinished(out bool result)
    {
        if ( Player.Distance >= Player.TotalDistance)
        {
            if (_RankPositionOfPlayer <= 3) result = true;

            else result = false;

            return true;
        }

        result = false;

        return false;
    }

    public override void RegisterResult(Action<bool> action)
    {
        RaceResult += action;
    }

    public override void InvokeResult(bool result)
    {
        RaceResult?.Invoke(result);
    }
}
