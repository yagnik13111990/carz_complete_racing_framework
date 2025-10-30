using System.Text;
using TMPro;
using UnityEngine;

public class TimeTrialHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text Timer;
    [SerializeField] private TMP_Text RankPosition;

    GameplayManager gameplayManager;

    StringBuilder builder;
    // Start is called before the first frame update
    void Start()
    {
        gameplayManager = FindFirstObjectByType<GameplayManager>();

        builder = new StringBuilder();
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

    void UpdateTimer()
    {
        builder.Clear();

        float time = gameplayManager.raceEvent.TimeLeft;

        int totalMilliseconds = Mathf.FloorToInt(time * 1000f);
        int minutes = (totalMilliseconds / 60000) % 60;
        int seconds = (totalMilliseconds / 1000) % 60;
        int milliseconds = totalMilliseconds % 1000;

        builder.AppendFormat("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);

        Timer.text = builder.ToString();
    }
}
