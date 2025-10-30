using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAI : ICar
{
    private GameObject _car;

    private AIDrivable _AiDrivable;
    private AIEntity _AiEntity;
    private AIPathTraker _AiPathTraker;


    public void Initialize(GameObject car)
    {
        _car = car;

        _AiEntity =  _car.AddComponent<AIEntity>();
        _AiEntity.enabled = false;

        _AiPathTraker =  _car.AddComponent<AIPathTraker>();
        _AiPathTraker.enabled = false;

        _AiDrivable = _car.AddComponent<AIDrivable>();
        _AiDrivable.enabled = false;
    }


}
