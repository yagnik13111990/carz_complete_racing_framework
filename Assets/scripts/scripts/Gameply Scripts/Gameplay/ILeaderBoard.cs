using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  interface ILeaderBoard 
{
    List<CarRaceStats> CarsInRace { get; set; }
    void SortTheList();
}
