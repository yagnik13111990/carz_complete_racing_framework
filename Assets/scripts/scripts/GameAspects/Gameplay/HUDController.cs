using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController: MonoBehaviour
{
    RaceModel raceModel;
    PlayerModel playerModel;
    RaceHUD raceHUD;
    PlayerHUD playerHUD;

#nullable enable
    ICommonCarEntity? commonCarEntity;
#nullable disable


    float unit;
    public void Initialize()
    {
        raceModel = new RaceModel();
        playerModel = new PlayerModel();

        raceHUD = FindObjectOfType<RaceHUD>();
        playerHUD = FindObjectOfType<PlayerHUD>();

        if (raceHUD != null)
        {
            raceHUD.OnResumeClick += ResumeGame;
            raceHUD.OnPauseClick += PausePanelActivation;
        }

        if (raceModel != null)
            raceModel.OnPauseMenuChanges += raceHUD.ShowPausePanel;

       

        GameStartEndEvent.OnRaceStarted += OnGameStart;
        GameStartEndEvent.OnRaceEnded += DisableHUDs;

        object obj = ServiceLocator.Instance.GetService<SettingManager>().M_HUDs.HUDsSettings[HUDsSettingKey.SpeedUnit];
        UnitOfSpeed unitOfSpeed = (UnitOfSpeed)Enum.ToObject(typeof(UnitOfSpeed), obj);
        
        if (unitOfSpeed == UnitOfSpeed.MPH)
        {
            unit = 0.6f;
            playerHUD.UpdateUnitText("MPH");
        }
        else
        {
            unit = 1f;
            playerHUD.UpdateUnitText("KPH");
        }
    }




    void OnGameStart()
    {
        StartCoroutine(InitializeCommonEntity());
    }

    IEnumerator InitializeCommonEntity()
    {

            yield return new WaitUntil(() => FindObjectOfType<DrivableCar>() != null);

            if (GameObject.FindGameObjectWithTag("Player").TryGetComponent<CommonEntity>(out CommonEntity CE))
            {
                commonCarEntity = CE;

                if (playerModel != null)
                    playerModel.OnCarElementChange += UpdateRaceCarUI;
        
            }

    }


    void UpdateRacePlayerElements()
    {
        if (commonCarEntity == null) return;
        playerModel.UpdateSpeed(commonCarEntity.Speed * unit);
       
    }

    void UpdateRaceCarUI(PlayerModel model)
    {
        if (commonCarEntity == null || playerHUD == null) return;
        playerHUD.UpdateSpeedometer(commonCarEntity.Speed * unit, commonCarEntity.MaximumSpeed * unit);
    }


    void ResumeGame()
    {
        raceModel?.SetPause(false);
        Time.timeScale = 1f;
       
    }

    void PausePanelActivation()
    {
        raceModel?.SetPause(true);
        Time.timeScale = 0f;
    }

    void DisableHUDs()
    {
        StartCoroutine(CloseAll());
    }


    void Update()
    { 
        UpdateRacePlayerElements();       
    }

    IEnumerator CloseAll()
    {
        if(this.gameObject == null) yield break;

        raceHUD.EnableMessageOfFinish(true);

        yield return new WaitForSeconds(2.7f);

        raceHUD.EnableMessageOfFinish(false);

        

    }

    private void OnDestroy()
    {
        if (raceHUD != null)
        {
            raceHUD.OnResumeClick -= ResumeGame;
            raceHUD.OnPauseClick -= PausePanelActivation;
            raceHUD.gameObject.SetActive(false);
        }

        if (raceModel != null)
            raceModel.OnPauseMenuChanges -= raceHUD.ShowPausePanel;


        if (playerModel != null)
            playerModel.OnCarElementChange -= UpdateRaceCarUI;


        if (playerHUD != null)
            playerHUD.gameObject.SetActive(false);

        GameStartEndEvent.OnRaceStarted -= OnGameStart;
        GameStartEndEvent.OnRaceEnded -= DisableHUDs;

    }


}


