using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SceneNotifier 
{
    public static event Action<string> OnAspectSelection;

    public static void NotifyToSceneMgmt(string Name )
    {
        OnAspectSelection?.Invoke(Name);
    }
}
