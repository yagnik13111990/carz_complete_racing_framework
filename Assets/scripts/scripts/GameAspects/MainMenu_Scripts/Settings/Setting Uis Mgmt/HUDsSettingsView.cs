using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public enum UnitOfSpeed : byte { KPH, MPH }

public class HUDsSettingsView : MonoBehaviour
{
    public CycleSelector<UnitOfSpeed> speedUnits;
   
    public List<ToggleUI> toggleUIs = new();

    private void Awake()
    {
        speedUnits.ContentSetUp(new EnumCycleSelecton<UnitOfSpeed>());
        toggleUIs = GetComponentsInChildren<ToggleUI>(true).ToList();       
    }

}
