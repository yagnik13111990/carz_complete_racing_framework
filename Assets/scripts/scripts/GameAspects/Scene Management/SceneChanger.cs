using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
   
    public void NotifySceneManager(string Name)
    {
        SceneNotifier.NotifyToSceneMgmt(Name);
    }
}
