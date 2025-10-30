using System;
using System.Linq;
using UnityEngine;

public class EliminationRace : RaceEvents
{
    private float eliminationTimeLimit = 10f;   // Time between eliminations
    private float _eliminationTimer;
    public override float EliminationTimer => _eliminationTimer;

    private float generalRaceTimeLimit = 60f;   // Delay before eliminations start
    private float generalRaceTime;

    public override float TimeBeforeElimination => generalRaceTime;

    private int _rankPositionOfPlayer;
    public override int RankPositionOfPlayer => _rankPositionOfPlayer;

    private float _raceTime;
    public override float RaceTimeOfPlayer => _raceTime;

    private int _remainingCars;
    public override int RemainingCars => _remainingCars;

    private CarRaceStats eliminatedCar;
    private CarRaceStats player;

    public event Action<string, int> OnCarEliminated;
    public event Action<bool> RaceResult;

    
    public override void Initialize(ILeaderBoard leaderBoard)
    {
        base.Initialize(leaderBoard);

        _remainingCars = _leaderBoard.CarsInRace.Count;
        _eliminationTimer = eliminationTimeLimit;
        generalRaceTime = generalRaceTimeLimit;

        player = _leaderBoard.CarsInRace.FirstOrDefault(c => c.Name == "You");

       
    }


    public override void Register(Action<string, int> action)
    {
        OnCarEliminated += action;
    }

    
    public override void Update()
    {
        if (_leaderBoard == null || _leaderBoard.CarsInRace.Count <= 1)
            return;

      
        _leaderBoard.SortTheList();

      
        if (player != null)
        {
            _rankPositionOfPlayer = _leaderBoard.CarsInRace.IndexOf(player) + 1;
            _raceTime = player.RaceTime;
        }

       
        if (generalRaceTime > 0)
        {
            generalRaceTime -= Time.deltaTime;
            _eliminationTimer = eliminationTimeLimit; 
            return; 
        }

      
        _eliminationTimer -= Time.deltaTime;

        if (_eliminationTimer <= 0f && _leaderBoard.CarsInRace.Count > 1)
        {
            EliminateLastCar();
            _eliminationTimer = eliminationTimeLimit;
        }
    }

    private void EliminateLastCar()
    {
        int lastIndex = _leaderBoard.CarsInRace.Count - 1;
        eliminatedCar = _leaderBoard.CarsInRace[lastIndex];

        if (eliminatedCar == null) return;

        _remainingCars--;
        OnCarEliminated?.Invoke(eliminatedCar.Name, _remainingCars);

      
        GameObject carObj = GameObject.Find(eliminatedCar.Name);
        if (carObj != null) GameObject.Destroy(carObj);

        _leaderBoard.CarsInRace.Remove(eliminatedCar);

        
    }

    // --- Race end checks ---
    public override bool isRaceFinished(out bool result)
    {
        result = false;

        
        if (_remainingCars == 1)
        {
            result = eliminatedCar != player;
            return true;
        }

      
        if (eliminatedCar == player)
        {
            result = false;
            return true;
        }

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
