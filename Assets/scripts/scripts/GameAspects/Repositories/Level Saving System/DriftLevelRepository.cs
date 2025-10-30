using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DriftLevelRepository : ILevelRepository
{
    private string FilePath => "GameJsonDataFiles/DriftLevelDatabase.json";

    public Dictionary<int, LevelData> LoadLevelData()
    {
        try
        {
            string path = Path.Combine(Application.persistentDataPath, FilePath);
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<Dictionary<int , LevelData>>(json);
        }

        catch
        {
           
            return new Dictionary<int, LevelData>();
        }
    }

    public void SaveLevelData(Dictionary<int, LevelData> levelDatas)
    {
        string path = Path.Combine(Application.persistentDataPath, FilePath);
        string json = JsonConvert.SerializeObject(levelDatas, Formatting.Indented);
        File.WriteAllText(path, json);
        
    }
}
