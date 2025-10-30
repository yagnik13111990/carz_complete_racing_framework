using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum RaceType : byte { SimpleRace , DriftRace}
public class LevePanelManagement : MonoBehaviour
{

    private Dictionary<int , LevelData> LevelDatas = new Dictionary<int , LevelData>();

    [SerializeField] private RaceType raceType;

    [SerializeField] private Transform Content;
    [SerializeField] private GameObject LevelPrefab;


    private void OnEnable()
    {
       

        if(LevelDatas.Count > 0) LevelDatas.Clear();
        if (raceType == RaceType.DriftRace)
        {
            LevelDatas = ServiceLocator.Instance.GetService<LevelManager>().DriftLevels;
        }

        else
        {
            LevelDatas = ServiceLocator.Instance.GetService<LevelManager>().RaceLevels;
        }

        foreach(KeyValuePair<int , LevelData> kvp in LevelDatas)
        {
            LevelData _data = kvp.Value;
            LevelData leveldata = new LevelDataBuilder().ID(_data.LevelID)
                .RaceType(raceType)
                .LevelState(_data.State)
                .RaceEvent(_data.RaceEvent)
                .Rank(_data.Rank)
                .StarCount(_data.Rating)
                .SceneName(_data.SceneName)
                .RewardAmount(_data.Reward)
                .SpawnAngleForCars(_data.SpawnAngle)
                .NumberOfCars(_data.NumberOfCars)
                .MaximumLaps(_data.MaxLap)
                .Build();
                
          
            GameObject levelobj = Instantiate(LevelPrefab, Content);
            LevelButton lvlbutton = levelobj.GetComponent<LevelButton>();
            lvlbutton.SetLevelData(leveldata);

        }
          
        
    }


   
}
