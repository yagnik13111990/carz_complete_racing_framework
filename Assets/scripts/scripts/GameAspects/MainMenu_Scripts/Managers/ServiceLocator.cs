using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    private static ServiceLocator instance;

    public static ServiceLocator Instance
    {
        get { return instance; }
    }

    private static Dictionary<Type, object> Services = new Dictionary<Type, object>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        
    }

    public void RegisterIntoService<T>(T Service) where T : class
    {
        Type type = typeof(T);

        Services[type] = Service;
        
    }

    public T GetService<T>() where T : class
    {
     
            if (Services.TryGetValue(typeof(T), out object service)) return service as T;
        

        return null;
    }

    public bool IsRegistered<T>() => Services.ContainsKey(typeof(T));
}



