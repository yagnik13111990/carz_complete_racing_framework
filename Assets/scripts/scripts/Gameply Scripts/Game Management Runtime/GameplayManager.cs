using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    IRaceRuleFactory raceRuleFactory;

    public RaceEvents raceEvent;
    public ILeaderBoard _leaderBoard;

    RaceEventHUDController raceEventHUDController;
    RaceHUD raceHUD;


    public bool HasGameStarted;
    public bool Checked;
    public bool HasRaceFinished;

    LevelManager levelManager;
    LevelData selectedLevel;

    [SerializeField] private GameObject Result;

    public int WinningPrize { get; private set; }

    public void Initialize()
    {
        _leaderBoard = FindFirstObjectByType<LeaderBoard>();

        raceRuleFactory = new RaceEventFactory();

        raceHUD = FindFirstObjectByType<RaceHUD>();
       
        raceEventHUDController = FindFirstObjectByType<RaceEventHUDController>();

        levelManager = ServiceLocator.Instance.GetService<LevelManager>();
        selectedLevel = levelManager.selectedLevel;

        raceEvent = raceRuleFactory.CreateRaceRule(selectedLevel.RaceEvent.ToString());

        StartCoroutine(DelayedInit());
    }

    IEnumerator DelayedInit()
    {
        yield return new WaitUntil(() => _leaderBoard.CarsInRace.Count > 0);
        raceEvent.Initialize(_leaderBoard);
    }

    public void Start()
    {
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        yield return new WaitUntil(() => _leaderBoard != null && _leaderBoard.CarsInRace.Count != 0);

        raceHUD.CountDown.text = "1";
        yield return new WaitForSeconds(1f);

        raceHUD.CountDown.text = "2";
        yield return new WaitForSeconds(1f);

        raceHUD.CountDown.text = "3";
        yield return new WaitForSeconds(1f);

        raceHUD.CountDown.text = "GO!";
        yield return new WaitForSeconds(1f);

        raceHUD.CountDown.enabled = false;

        raceEventHUDController.RaceRuleCanvases[ServiceLocator.Instance.GetService<LevelManager>().selectedLevel.RaceEvent].gameObject.SetActive(true);

        GameStartEndEvent.OnRaceStartedInvokation();

        HasGameStarted = true;

        //  Start race end management once
        StartCoroutine(RaceEndManagement());
    }

    void Update()
    {
        if (!HasGameStarted) return;
        if (_leaderBoard == null || _leaderBoard.CarsInRace.Count == 0) return;
        if (HasRaceFinished) return;

       
        raceEvent.Update();

       
    }

    IEnumerator RaceEndManagement()
    {
        //  Loop safely each frame until race finishes
        while (!HasRaceFinished)
        {
            if (raceEvent.isRaceFinished(out bool RaceResult) && !Checked)
            {
                HasRaceFinished = true;

                WinningPrize = Mathf.RoundToInt(selectedLevel.Reward / raceEvent.RankPositionOfPlayer);

                yield return new WaitForSeconds(1f);

                raceEventHUDController.RaceRuleCanvases[ServiceLocator.Instance.GetService<LevelManager>().selectedLevel.RaceEvent].gameObject.SetActive(false);
                Result.gameObject.SetActive(true);

                yield return new WaitForSeconds(1.5f);

                raceEvent.InvokeResult(RaceResult);
                GameStartEndEvent.OnRaceFinishedInvokation();

                if (RaceResult == true)
                {
                    if (selectedLevel.raceType == RaceType.DriftRace)
                    {
                        if (selectedLevel.Rank != 0 && selectedLevel.Rank < raceEvent.RankPositionOfPlayer) yield break;

                        selectedLevel.Rank = raceEvent.RankPositionOfPlayer;
                        selectedLevel.State = State.IsCompleted;
                        levelManager.DriftLevels[selectedLevel.LevelID] = selectedLevel;

                        if (levelManager.DriftLevels[selectedLevel.LevelID + 1].State == State.Locked)
                            levelManager.DriftLevels[selectedLevel.LevelID + 1].State = State.Unlocked;

                        levelManager.DriftLevelRepository.SaveLevelData(levelManager.DriftLevels);
                    }
                    else if (selectedLevel.raceType == RaceType.SimpleRace)
                    {
                        if (selectedLevel.Rank != 0 && selectedLevel.Rank < raceEvent.RankPositionOfPlayer) yield break;

                        selectedLevel.Rank = raceEvent.RankPositionOfPlayer;
                        selectedLevel.State = State.IsCompleted;
                        levelManager.RaceLevels[selectedLevel.LevelID] = selectedLevel;

                        if (levelManager.RaceLevels[selectedLevel.LevelID + 1].State == State.Locked)
                            levelManager.RaceLevels[selectedLevel.LevelID + 1].State = State.Unlocked;

                        levelManager.SimpleLevelRepository.SaveLevelData(levelManager.RaceLevels);
                    }
                }

                ServiceLocator.Instance.GetService<WalletManager>().Addcoin(WinningPrize);
                Checked = true;
                selectedLevel = null;
            }

            yield return null; // wait for next frame (non-blocking)
        }
    }
}
