using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel
{
 
    public float Speed { get; private set; }
    
    public Action<PlayerModel> OnCarElementChange;
  
    public void UpdateSpeed(float value)
    {
        this.Speed = value;
      
        NotifyChangeInElements();
    }

    public void NotifyChangeInElements()
    {
        OnCarElementChange?.Invoke(this);
    }

  
}


