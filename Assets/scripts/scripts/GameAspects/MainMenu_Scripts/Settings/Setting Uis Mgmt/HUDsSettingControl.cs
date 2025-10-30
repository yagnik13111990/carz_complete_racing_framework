using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class HUDsSettingsControl : MonoBehaviour
{
    private HUDsSettingsView V_HUDs;
    private HUDsSettingModel M_HUDs;

    private bool Initialized;

    private HUDsSettingKey[] keys;
    // Start is called before the first frame update
    void Start()
    {
        if (Initialized) return;
        Initialized = true;

        V_HUDs = FindFirstObjectByType<HUDsSettingsView>();       
        M_HUDs = ServiceLocator.Instance.GetService<SettingManager>().M_HUDs;

        keys = (HUDsSettingKey[])Enum.GetValues(typeof(HUDsSettingKey));

        ToggleValueBinding();
        SpeedUnitUpdation();
    }

   

    private void ToggleValueBinding()
    {
        // Get all keys except SpeedUnit
        var toggleKeys = keys.Where(k => k != HUDsSettingKey.SpeedUnit).ToList();

        for (int i = 0; i < toggleKeys.Count ; i++)
        {
            int index = i;

            HUDsSettingKey key = toggleKeys[index];

            bool currentValue = false;
            if (M_HUDs.HUDsSettings.TryGetValue(key, out var val))
                currentValue = Convert.ToBoolean(val);

            V_HUDs.toggleUIs[index].SetValue(currentValue);

            // UI → Model
            V_HUDs.toggleUIs[index].OnToggleChanged += isOn =>
            {
                M_HUDs.SetSetting(key, isOn);
            };

            // Model → UI
            M_HUDs.OnSettingDataChange += (_key, newVal) =>
            {
                if (_key == key && V_HUDs.toggleUIs[index].IsOn != Convert.ToBoolean(newVal))
                {
                    V_HUDs.toggleUIs[index].SetValue(Convert.ToBoolean(newVal));
                }
                   
            };
        }
    }


    void SpeedUnitUpdation()
    {
        object obj = M_HUDs.HUDsSettings[HUDsSettingKey.SpeedUnit];
        UnitOfSpeed unit = (UnitOfSpeed) Enum.ToObject(typeof(UnitOfSpeed) , obj);

        V_HUDs.speedUnits.SetValue(unit);

        V_HUDs.speedUnits.OnValueSelection += (val) => { M_HUDs.SetSetting(HUDsSettingKey.SpeedUnit, val); };

        M_HUDs.OnSettingDataChange += (key, val) => 
        { 
            if (key == HUDsSettingKey.SpeedUnit && V_HUDs.speedUnits.CurrentSelected != (UnitOfSpeed)Enum.ToObject(typeof(UnitOfSpeed), val)) 
            {
                V_HUDs.speedUnits.SetValue((UnitOfSpeed)Enum.ToObject(typeof(UnitOfSpeed), val));
            } 
        };

    }


}
