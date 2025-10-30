using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SprintRace : RaceEvents
{

    public int _positionOfPlayer {  get; private set; }
    public override int RankPositionOfPlayer => _positionOfPlayer;

    public float _TotalDistance;
    public override float TotalDistance => _TotalDistance;
    public float _DistanceCovered { get; private set; }
    public override float DistanceCovered => _DistanceCovered;

    public float _Timer { get; private set; }
    public override float RaceTimeOfPlayer => _Timer;


    private CarRaceStats Player;

    private event Action<bool> RaceResult;

    private bool isInvoked;

    public override void Initialize(ILeaderBoard leaderBoard)
    {
        base.Initialize(leaderBoard);

        Player = _leaderBoard.CarsInRace.FirstOrDefault(a => a.Name == "You") ;

        _TotalDistance = Player.TotalDistance;  
    }

   
    public override void Update()
    {
        if (_leaderBoard == null || _leaderBoard.CarsInRace.Count == 0 )
            return;

        _leaderBoard.SortTheList();
      
        _DistanceCovered = Player.Distance;

        _Timer = Player.RaceTime;

        

        if (Player != null)
        {
            _positionOfPlayer = _leaderBoard.CarsInRace.IndexOf(Player) + 1;
   
        }
           
    }

    public override bool isRaceFinished(out bool result)
    {
       if(_DistanceCovered >= Player.TotalDistance && !isInvoked)
       {
            
            isInvoked = true;

            
            if (_positionOfPlayer <= 3 ) result = true;

            else result = false;

            return true;
       }
        result = false ;
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
