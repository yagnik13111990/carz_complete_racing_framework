using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FirstToFinishHUD : MonoBehaviour
{
    [SerializeField] private Slider ProgressBar;

    [SerializeField] private TMP_Text DistanceCovered;
    [SerializeField] private TMP_Text RankPosition;

    [SerializeField] private TMP_Text Timer;

    StringBuilder builder;

    GameplayManager gameplayManager;

    ITrackUtility trackUtility;

    // Start is called before the first frame update
    void Start()
    {
        trackUtility = FindFirstObjectByType<TrackUtility>();


        builder = new StringBuilder();

        gameplayManager = FindFirstObjectByType<GameplayManager>();
       
        ProgressBar.maxValue = gameplayManager.raceEvent.TotalDistance;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDistance();
        UpdateRankPosition();
        UpdateTimer();
        UpdateProgressBar();
    }

    void UpdateDistance()
    {
        builder.Clear();

        builder.Append(Math.Round(gameplayManager.raceEvent.DistanceCovered , 2));
        builder.Append(" M");

        DistanceCovered.text = builder.ToString();

    }

    void UpdateRankPosition()
    {

        builder.Clear();

        builder.Append(gameplayManager.raceEvent.RankPositionOfPlayer);
        builder.Append("/");
        builder.Append(ServiceLocator.Instance.GetService<LevelManager>().selectedLevel.NumberOfCars);

        RankPosition.text = builder.ToString() ;

    }

    void UpdateTimer()
    {
        builder.Clear();

       
        float time = gameplayManager.raceEvent.RaceTimeOfPlayer;

        int totalMilliseconds = Mathf.FloorToInt(time * 1000f);
        int minutes = (totalMilliseconds / 60000) % 60;
        int seconds = (totalMilliseconds / 1000) % 60;
        int milliseconds = totalMilliseconds % 1000;

        builder.AppendFormat("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);

        Timer.text = builder.ToString();
    }


    void UpdateProgressBar()
    {
        ProgressBar.value = gameplayManager.raceEvent.DistanceCovered;
    }
}
