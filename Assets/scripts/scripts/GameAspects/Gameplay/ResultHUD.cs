using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class ResultHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text WinText;
    [SerializeField] private TMP_Text LoseText;

    [SerializeField] private TMP_Text Rank;
    [SerializeField] private TMP_Text Prize;
    [SerializeField] private TMP_Text RaceTime;
    [SerializeField] private TMP_Text isQualified;
    
    [SerializeField] private Transform Content;

    [SerializeField] private GameObject LeaderBoardItem;
    [SerializeField] private GameObject LeaderBoard;

   
    GameplayManager gameplayManager;

    StringBuilder builder;

    CarRaceStats Player;

    LevelData selectedLevel;

    LevelManager levelManager;

    float FakeTime = 0f;

    private void Start()
    {
        levelManager = ServiceLocator.Instance.GetService<LevelManager>();
        selectedLevel = levelManager.selectedLevel;

        builder = new StringBuilder();

        gameplayManager = FindFirstObjectByType<GameplayManager>();
        gameplayManager.raceEvent.RegisterResult(HandleResult);

        Player = gameplayManager._leaderBoard.CarsInRace.FirstOrDefault(a => a.Name == "You");
       
    }

    

    private void HandleResult(bool HasWin)
    {
   

        UpdateTimer(gameplayManager.raceEvent.RaceTimeOfPlayer, RaceTime);
        UpdatePosition(gameplayManager.raceEvent.RankPositionOfPlayer);
        isQualified.gameObject.SetActive(true);

        switch (selectedLevel.RaceEvent)
        {
            case RaceEvent.LapRace:
            case RaceEvent.TimeDown:
            case RaceEvent.Sprint:

                gameplayManager._leaderBoard.SortTheList();

                LeaderBoard.SetActive(true);

                if (HasWin)
                {
                    selectedLevel.State = State.IsCompleted;

                    Prize.text = "Winning Prize - " + gameplayManager.WinningPrize.ToString() + " CR";

                    Prize.gameObject.SetActive(true);

                    isQualified.text = "YOU ARE QUALIFIED FOR NEXT RACE !";
                    isQualified.color = Color.green;

                }

                int index = gameplayManager._leaderBoard.CarsInRace.IndexOf(Player);

                for (int i = 0; i <= index ; i++)
                {
                    int ind = i;

                    CarRaceStats data = gameplayManager._leaderBoard.CarsInRace[ind];

                    GameObject obj = Instantiate(LeaderBoardItem, Content);

                    ResultItem item = obj.GetComponent<ResultItem>();

                    item.Rank.text = (ind + 1).ToString();
                    item.Name.text = data.Name;

                    if (selectedLevel.RaceEvent == RaceEvent.Sprint || selectedLevel.RaceEvent == RaceEvent.LapRace)
                        UpdateTimer(data.RaceTime, item.Stats);

                    else item.Stats.text = "Distance - " + string.Format("{0:F2}", data.Distance);

                    if (data == Player)
                    {
                        item.Rank.color = Color.green;
                        item.Name.color = Color.green;
                        item.Stats.color = Color.green;
                        item.BG.color = Color.yellow;
                    }


                }

                if(index < gameplayManager._leaderBoard.CarsInRace.Count - 1)
                {
                    for (int i = index + 1; i <= gameplayManager._leaderBoard.CarsInRace.Count - 1; i++)
                    {
                        int ind = i;

                        CarRaceStats data = gameplayManager._leaderBoard.CarsInRace[ind];

                        GameObject obj = Instantiate(LeaderBoardItem, Content);

                        ResultItem item = obj.GetComponent<ResultItem>();

                        item.Rank.text = (ind + 1).ToString();
                        item.Name.text = data.Name;

                        FakeTime += Player.RaceTime +  ind * Random.Range(4, 10);
                       
                        if (selectedLevel.RaceEvent == RaceEvent.Sprint || selectedLevel.RaceEvent == RaceEvent.LapRace)
                            UpdateTimer(FakeTime, item.Stats);

                        else item.Stats.text = "Distance - " + string.Format("{0:F2}", data.Distance);



                    }
                }

                else
                {
                    isQualified.text = "SORRY ! YOU DIDN'T QUALIFY FOR NEXT RACE !";
                    isQualified.color = Color.red;
                }

            break;

            case RaceEvent.OneVOne:
            case RaceEvent.Elimination:

                if (HasWin)
                {
                    selectedLevel.State = State.IsCompleted;

                    WinText.gameObject.SetActive(true);

                    Prize.text = "Winning Prize - " + gameplayManager.WinningPrize.ToString() + " CR";

                    Prize.gameObject.SetActive(true);


                    isQualified.text = "YOU ARE QUALIFIED FOR NEXT RACE !";

                    isQualified.color = Color.green;
                }

                else
                {
                    LoseText.text = "YOU LOST!!!";

                    LoseText.gameObject.SetActive(true);

                    isQualified.text = "SORRY ! YOU DIDN'T QUALIFY FOR NEXT RACE !";

                    isQualified.color = Color.red;
                }

            break;
        }

        
    }

    void UpdateTimer(float Time , TMP_Text Text)
    {
        builder.Clear();

        float time = Time;

        int totalMilliseconds = Mathf.FloorToInt(time * 1000f);
        int minutes = (totalMilliseconds / 60000) % 60;
        int seconds = (totalMilliseconds / 1000) % 60;
        int milliseconds = totalMilliseconds % 1000;

        builder.Append("RACE TIME - "); 
        builder.AppendFormat("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);

        Text.text = builder.ToString();
    }

    void UpdatePosition(int rank)
    {
        builder.Clear();

        builder.Append("RANK POSITION - ");
        builder.Append(rank);

        string rankFormat = rank switch { 1 => "ST", 2 => "ND", 3 => "RD", _ => "TH" };

        builder.Append(rankFormat);

        Rank.text = builder.ToString();
        
    }

   
}
