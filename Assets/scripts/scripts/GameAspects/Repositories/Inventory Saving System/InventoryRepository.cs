using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;


public class InventoryRepository : IInventoryRepository
{
    private string FilePath = "GameJsonDataFiles/AvailableCarsDatabase.json";

    public List<CarInfo> LoadAvailableCarData()
    {
        try
        {
            string path = Path.Combine(Application.persistentDataPath, FilePath);
            string json = File.ReadAllText(path);

            return JsonConvert.DeserializeObject<List<CarInfo>>(json);

        }

        catch
        {
            Debug.LogError("json is empty");
            return new List<CarInfo>();
        }
    }

    public void SaveAvailableCarData(List<CarInfo> info)
    {
        string path = Path.Combine(Application.persistentDataPath, FilePath);
        string json = JsonConvert.SerializeObject(info , Formatting.Indented);

        File.WriteAllText(path, json );
    }

   
}
