using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelRepository 
{
    void SaveLevelData(Dictionary<int, LevelData> levelDatas);

    Dictionary<int, LevelData> LoadLevelData();
}
