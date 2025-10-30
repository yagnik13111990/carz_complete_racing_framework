using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeaderBoard : MonoBehaviour , ILeaderBoard
{

    private List<CarRaceStats> _CarsInRace = new List<CarRaceStats>();

    public List<CarRaceStats> CarsInRace { get => _CarsInRace; set => _CarsInRace = value;  }

    private void OnEnable()
    {
        SpawnEvent.OnSpawnEvent += RegisterCarToList;
    }

    private void OnDisable()
    {
        SpawnEvent.OnSpawnEvent -= RegisterCarToList;
    }
   

    public void RegisterCarToList(CarRaceStats car)
    {
        _CarsInRace.Add(car);
    }
  

    public void SortTheList()
    {
       _CarsInRace.Sort(CompareCars);
    }

    int CompareCars(CarRaceStats car1 , CarRaceStats car2)
    {
        
        return car2.Distance.CompareTo(car1.Distance);
      
    }
}
