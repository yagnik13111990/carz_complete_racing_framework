using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceModel  
{

    public bool IsPaused { get; private set; }

    public event Action<bool> OnPauseMenuChanges;


    public void SetPause(bool paused) { IsPaused = paused; OnPauseMenuChanges?.Invoke(IsPaused); }

    
   
 
    
   



}
