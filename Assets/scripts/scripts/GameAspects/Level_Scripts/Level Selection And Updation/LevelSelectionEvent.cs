using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelSelectionEvent 
{
    public static event Action<LevelData> OnLevelSelected;

    public static void NotifyLevelManager(LevelData data)
    {
        OnLevelSelected?.Invoke(data);
    }
}
