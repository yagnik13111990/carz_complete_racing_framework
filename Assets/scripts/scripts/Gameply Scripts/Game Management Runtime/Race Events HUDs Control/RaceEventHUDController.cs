using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceEventHUDController : MonoBehaviour
{

    [SerializeField] private Canvas EliminationCanvas;
    [SerializeField] private Canvas SprintCanvas;
    [SerializeField] private Canvas OneVOneCanvas;
    [SerializeField] private Canvas TimeTrialCanvas;
    [SerializeField] private Canvas LapRaceCanvas;

    public Dictionary<RaceEvent, Canvas> RaceRuleCanvases;

    private void Awake()
    {
        RaceRuleCanvases = new Dictionary<RaceEvent, Canvas>()
        {
            {RaceEvent.Elimination, EliminationCanvas},
            {RaceEvent.Sprint, SprintCanvas},
            {RaceEvent.OneVOne , OneVOneCanvas},
            {RaceEvent.TimeDown, TimeTrialCanvas},
            {RaceEvent.LapRace, LapRaceCanvas}
        };
    }
    

   
}
