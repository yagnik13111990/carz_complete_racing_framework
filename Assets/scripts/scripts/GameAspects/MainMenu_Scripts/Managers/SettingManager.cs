using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager 
{
    public ControlSettingModel M_Control;
    public GameplaySettingModel M_Gameplay;
    public HUDsSettingModel M_HUDs;

    public ISettingRepository<HUDsSettingKey , object> HUDsSettingRepository;
    public ISettingRepository<GameplaySettingKey, object> GameplaySettingRepository;
    public ISettingRepository<ControlSettingKey, KeyCode> ControlSettingRepository;
    public SettingManager()
    {
        HUDsSettingRepository = new HUDsSettingRepository();
        GameplaySettingRepository = new GameplaySettingRepository();
        ControlSettingRepository = new ControlSettingRepository();



        M_HUDs = new HUDsSettingModel();
        M_Gameplay = new GameplaySettingModel();
        M_Control = new ControlSettingModel(ControlSettingRepository.LoadSettings());

        M_HUDs.HUDsSettings = HUDsSettingRepository.LoadSettings();
        M_Gameplay.GameplaySettings = GameplaySettingRepository.LoadSettings();
       
     

       
    }
}
