using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum HUDsSettingKey :byte { Position, WrongWayWarning, SpeedoMeter, OffTrackWarning, NearByCarsDist , LapCounter , SpeedUnit }
public class HUDsSettingModel 
{
    private Dictionary<HUDsSettingKey , object> _HUDsSettings = new Dictionary<HUDsSettingKey , object>()
    {
        {HUDsSettingKey.Position , true },
        {HUDsSettingKey.SpeedoMeter , true },
        {HUDsSettingKey.OffTrackWarning, true },
        {HUDsSettingKey.NearByCarsDist , true },
        {HUDsSettingKey.LapCounter , true },
        {HUDsSettingKey.SpeedUnit , UnitOfSpeed.KPH}

    };
    public Dictionary<HUDsSettingKey, object> HUDsSettings { get { return _HUDsSettings; } set { _HUDsSettings = value; } }

   

    public event Action<HUDsSettingKey, object> OnSettingDataChange;
    public void SetSetting(HUDsSettingKey key , object val)
    {
        if (HUDsSettings[key].Equals(val)) return;

        HUDsSettings[key] = val;

        OnSettingDataChange?.Invoke(key, val);

        ServiceLocator.Instance.GetService<SettingManager>().HUDsSettingRepository.SaveSettings(_HUDsSettings);

    }
   
    
}
