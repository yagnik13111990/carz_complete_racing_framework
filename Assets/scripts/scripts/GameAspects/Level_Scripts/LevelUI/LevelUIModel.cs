using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUIModel 
{
    public bool IsRaceSelectionCanvasEnabled {  get; private set; }
    public bool IsLevelSelectionCanvasEnabled { get; private set; }
    public bool IsDriftLevelSelected { get; private set; }
    public bool IsSimpleRaceLevelSelected { get; private set; }
    // Start is called before the first frame update

    public event Action<LevelUIModel> OnValueChange;

    public void UpdateRaceSelectionActivation(bool Enable)
    {
        IsRaceSelectionCanvasEnabled = Enable;
        OnValueChange?.Invoke(this);
    }
    public void UpdateLevelSelectionActivation(bool Enable)
    {
        IsLevelSelectionCanvasEnabled = Enable;
        OnValueChange?.Invoke(this);
    }
    public void UpdateDriftLevelsActivation(bool Enable)
    {
        IsDriftLevelSelected = Enable;
        OnValueChange?.Invoke(this);
    }
    public void UpdateSimpleRaceLevelsActivation(bool Enable)
    {
        IsSimpleRaceLevelSelected = Enable;
        OnValueChange?.Invoke(this);

    }
}
