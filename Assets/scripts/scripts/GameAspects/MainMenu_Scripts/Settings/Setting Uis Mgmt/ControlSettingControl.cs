using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ControlSettingControl : MonoBehaviour
{
    private ControlSettingModel M_Control;
    private ControlSettingView V_Control;

    private ControlSettingKey[] keys;
    private bool Initialized;

    private void Start()
    {
        if (Initialized) return;
        Initialized = true;

        M_Control = ServiceLocator.Instance.GetService<SettingManager>().M_Control;
        V_Control = FindFirstObjectByType<ControlSettingView>();

        keys = (ControlSettingKey[])Enum.GetValues(typeof(ControlSettingKey));

        UpdateInputSelection();
    }

    private void UpdateInputSelection()
    {
        if (keys == null || V_Control.InputCycleSelectors == null) return;

        for (int i = 0; i < keys.Length; i++)
        {
            int index = i;
            ControlSettingKey key = keys[index];

            // safely get object from dictionary
            if (!M_Control.ControlSettings.TryGetValue(key, out var obj))
            {
                obj = KeyCode.None;
                M_Control.ControlSettings[key] = obj;
            }

            KeyCode currentValue = obj;
            V_Control.InputCycleSelectors[index].SetValue(currentValue);

            // UI → Model
            V_Control.InputCycleSelectors[index].OnValueSelection += (val) => M_Control.SetSettings(key, val);

            // Model → UI
            M_Control.ControlSettingChanged += (_key, _val) =>
            {
                if (_key != key) return;

                KeyCode val =_val;

                if (V_Control.InputCycleSelectors[index].CurrentSelected == val) return;

                V_Control.InputCycleSelectors[index].SetValue(val);
            };
        }
    }
}
