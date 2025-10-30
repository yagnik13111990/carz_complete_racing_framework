using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Rendering;
using UnityEngine;

public class HUDsSettingRepository : ISettingRepository<HUDsSettingKey , object>
{
    string FilePath => Path.Combine(Application.persistentDataPath, "GameJsonDataFiles/HUDsSettings.json");
    public Dictionary<HUDsSettingKey, object> LoadSettings()
    {
        try
        {
            if(!File.Exists(FilePath)) return new Dictionary<HUDsSettingKey, object>();

            string json = File.ReadAllText(FilePath);

            var result =  JsonConvert.DeserializeObject<Dictionary<HUDsSettingKey, object>>(json);

            return result ?? new Dictionary<HUDsSettingKey, object>();
        }

        catch
        {      
                return new Dictionary<HUDsSettingKey, object>();
            
        }
    }

  

    public void SaveSettings(Dictionary<HUDsSettingKey, object> settings)
    {      
            string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        
    }
}
