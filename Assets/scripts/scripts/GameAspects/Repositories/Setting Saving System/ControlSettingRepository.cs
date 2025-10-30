using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ControlSettingRepository : ISettingRepository<ControlSettingKey, KeyCode>
{
    private string FilePath => Path.Combine(Application.persistentDataPath, "GameJsonDataFiles/ControlSettings.json");

    public Dictionary<ControlSettingKey, KeyCode> LoadSettings()
    {
        try
        {
            if (!File.Exists(FilePath))
                return GetDefaultSettings();

            string json = File.ReadAllText(FilePath);
            var result = JsonConvert.DeserializeObject<Dictionary<ControlSettingKey, KeyCode>>(json);

            // fallback to default if json is invalid
            return result ?? GetDefaultSettings();
        }
        catch
        {
            return GetDefaultSettings();
        }
    }

    public void SaveSettings(Dictionary<ControlSettingKey, KeyCode> settings)
    {
        string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
        File.WriteAllText(FilePath, json);
    }

    private Dictionary<ControlSettingKey, KeyCode> GetDefaultSettings()
    {
        return new Dictionary<ControlSettingKey, KeyCode>
        {
            { ControlSettingKey.Acceleration, KeyCode.UpArrow },
            { ControlSettingKey.Reverse, KeyCode.DownArrow },
            { ControlSettingKey.SteerLeft, KeyCode.LeftArrow },
            { ControlSettingKey.SteerRight, KeyCode.RightArrow },
            { ControlSettingKey.SwitchCam, KeyCode.C },
            { ControlSettingKey.ShortBrake, KeyCode.Space }           
        };
    }
}

