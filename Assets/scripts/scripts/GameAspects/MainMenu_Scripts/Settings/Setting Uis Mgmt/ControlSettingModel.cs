using System;
using System.Collections.Generic;
using UnityEngine;

public enum ControlSettingKey : byte
{
    Acceleration, Reverse, SteerLeft, SteerRight, SwitchCam, ShortBrake
}

public class ControlSettingModel
{
    private Dictionary<ControlSettingKey, KeyCode> _ControlSettings;
    public Dictionary<ControlSettingKey, KeyCode> ControlSettings
    {
        get => _ControlSettings;
        set => _ControlSettings = value ?? new Dictionary<ControlSettingKey, KeyCode>();
    }

    public event Action<ControlSettingKey, KeyCode> ControlSettingChanged;

    public ControlSettingModel(Dictionary<ControlSettingKey, KeyCode> initialSettings)
    {
        _ControlSettings = initialSettings ?? new Dictionary<ControlSettingKey, KeyCode>();
    }

    public void SetSettings(ControlSettingKey key, KeyCode value)
    {
        if (_ControlSettings.ContainsKey(key) && _ControlSettings[key] == value)
            return;

        _ControlSettings[key] = value;
        ControlSettingChanged?.Invoke(key, value);

        // Save immediately
        ServiceLocator.Instance.GetService<SettingManager>().ControlSettingRepository.SaveSettings(_ControlSettings);
    }
}
