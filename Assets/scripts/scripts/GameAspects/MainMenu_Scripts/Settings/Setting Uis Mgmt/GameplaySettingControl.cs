using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameplaySettingControl : MonoBehaviour
{
    GameplaySettingModel M_Gameplay;
    GameplaySettingView V_Gameplay;

    private GameplaySettingKey[] keys;

    private bool Initialized;


    // Start is called before the first frame update
    void Start()
    {
        if (Initialized) return;
        Initialized = true;

        M_Gameplay = ServiceLocator.Instance.GetService<SettingManager>().M_Gameplay;
        V_Gameplay = FindFirstObjectByType<GameplaySettingView>();

        keys = (GameplaySettingKey[]) Enum.GetValues(typeof(GameplaySettingKey));

      
        SliderValueUpdation();
        ToggleValueBinding();
    }
    private void ToggleValueBinding()
    {
        var toggleKeys = keys.Where(k => k != GameplaySettingKey.CamFollowMultiplier).ToList();
       
        for (int i = 0; i < toggleKeys.Count; i++)
        {
            int index = i;
            GameplaySettingKey key = toggleKeys[index];

            bool currentValue = false;
            if (M_Gameplay.GameplaySettings.TryGetValue(key, out var val))
                currentValue = Convert.ToBoolean(val);

            V_Gameplay.toggleUIs[index].SetValue(currentValue);

            // UI -> Model
            V_Gameplay.toggleUIs[index].OnToggleChanged += isOn =>
            {
                M_Gameplay.SetSettings(key, isOn);
            };

            // Model -> UI
            M_Gameplay.GameplaySettingChanged += (_key, newVal) =>
            {
                if (_key == key)
                {
                    if (V_Gameplay.toggleUIs[index].IsOn != Convert.ToBoolean(newVal))
                    {
                        V_Gameplay.toggleUIs[index].SetValue(Convert.ToBoolean(newVal)); 
                    }
                   
                }
            };
        }
    }


    void SliderValueUpdation()
    {
        object obj = M_Gameplay.GameplaySettings[GameplaySettingKey.CamFollowMultiplier];
        float value = Convert.ToSingle(obj);

        V_Gameplay.CamFollowSlider.SetValue(value);

        // UI -> Model
        V_Gameplay.CamFollowSlider.SliderChanged += val =>
        {
            M_Gameplay.SetSettings(GameplaySettingKey.CamFollowMultiplier, val);
        };

        // Model -> UI
        M_Gameplay.GameplaySettingChanged += (key, val) =>
        {
            if (key == GameplaySettingKey.CamFollowMultiplier && V_Gameplay.CamFollowSlider.Value != Convert.ToSingle(val))
                V_Gameplay.CamFollowSlider.SetValue(Convert.ToSingle(val));
        };
    }

}
