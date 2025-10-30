using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameplaySettingRepository : ISettingRepository<GameplaySettingKey, object>
{
    string FilePath => Path.Combine(Application.persistentDataPath, "GameJsonDataFiles/GameplaySettings.json");
    public Dictionary<GameplaySettingKey, object> LoadSettings()
    {
        try
        {
            if (!File.Exists(FilePath)) return new Dictionary<GameplaySettingKey, object>();

            string json = File.ReadAllText(FilePath);

            var result = JsonConvert.DeserializeObject<Dictionary<GameplaySettingKey, object>>(json);

            return result ?? new Dictionary<GameplaySettingKey, object>();
        }

        catch {
            return new Dictionary<GameplaySettingKey, object>();
        }
    }

    public void SaveSettings(Dictionary<GameplaySettingKey, object> settings)
    {
        string json = JsonConvert.SerializeObject(settings , Formatting.Indented);
        File.WriteAllText(FilePath , json);
    }
}
