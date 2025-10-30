using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenSingleton : MonoBehaviour
{
    
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    
}
