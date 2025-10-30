using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPlayer : ICar
{
    private GameObject _car;

    private DrivableCar _drivableCar;

    private ControlHelper _controlHelper;
    public void Initialize(GameObject car)
    {
        _car = car;
      
        if ((bool)ServiceLocator.Instance.GetService<SettingManager>().M_Gameplay.GameplaySettings[GameplaySettingKey.HarderHandling] == true)
        {
          
            _controlHelper = _car.AddComponent<ControlHelper>();
            _controlHelper.enabled = false;

        }

       _drivableCar =  _car.AddComponent<DrivableCar>();
       _drivableCar.enabled = false;
    }

   
}
