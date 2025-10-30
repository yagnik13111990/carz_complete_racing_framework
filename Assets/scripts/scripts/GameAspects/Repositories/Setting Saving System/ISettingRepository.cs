using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISettingRepository<Tkey , TValue> where Tkey : Enum
{
    void SaveSettings(Dictionary<Tkey, TValue> settings);

    Dictionary<Tkey, TValue> LoadSettings();


   


}
