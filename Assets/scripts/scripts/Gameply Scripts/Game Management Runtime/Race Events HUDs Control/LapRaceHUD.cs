using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class LapRaceHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text Timer;

    [SerializeField] private TMP_Text RankPosition;
    [SerializeField] private TMP_Text LapCount;

    [SerializeField] private TMP_Text NotifyLapChange;

    GameplayManager gameplayManager;

    StringBuilder builder;
    // Start is called before the first frame update
    void Start()
    {
        builder = new StringBuilder();

        gameplayManager = FindFirstObjectByType<GameplayManager>();

        gameplayManager.raceEvent.Register(NotifyLapChangeAndUpdate);

        LapCount.text = $"1 / {ServiceLocator.Instance.GetService<LevelManager>().selectedLevel.MaxLap}";
    }

    
    // Update is called once per frame
    void Update()
    {
        UpdateRankPosition();
        UpdateTimer();
    }

    void UpdateRankPosition()
    {
        builder.Clear();

        builder.Append(gameplayManager.raceEvent.RankPositionOfPlayer);
        builder.Append("/");
        builder.Append(ServiceLocator.Instance.GetService<LevelManager>().selectedLevel.NumberOfCars);

        RankPosition.text = builder.ToString();
    }
    void NotifyLapChangeAndUpdate(string val , int lap)
    {
        LapCount.text = $"{lap} / {ServiceLocator.Instance.GetService<LevelManager>().selectedLevel.MaxLap}";

        StartCoroutine(Notify(val));
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

    IEnumerator Notify(string val)
    {
        NotifyLapChange.text = val;
        NotifyLapChange.gameObject.SetActive(true);

        if (this == null)
        {
            NotifyLapChange.gameObject.SetActive(false);
            
            yield break;
        }

        yield return new WaitForSeconds(3f);

        NotifyLapChange.gameObject.SetActive(false);
    }
}
