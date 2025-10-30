using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameplaySettingKey : byte {HarderHandling , Map , Notification , AutoDrift , CamFollowMultiplier }
public class GameplaySettingModel 
{
    private Dictionary<GameplaySettingKey , object> _GameplaySettings  = new Dictionary<GameplaySettingKey , object>();
    public Dictionary<GameplaySettingKey, object> GameplaySettings { get => _GameplaySettings; set => _GameplaySettings = value; }

   

    public event Action<GameplaySettingKey , object> GameplaySettingChanged;

    public void SetSettings(GameplaySettingKey key , object val)
    {
        if (_GameplaySettings[key] == val) return;

        _GameplaySettings[key] = val;

        GameplaySettingChanged?.Invoke(key, val);

        ServiceLocator.Instance.GetService<SettingManager>().GameplaySettingRepository.SaveSettings(_GameplaySettings);
    }


}
