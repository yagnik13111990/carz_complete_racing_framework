using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarFactory : ICarFactory
{
    private GameObject _car;

    private ICar PlayerCar;
    public GameObject CreateCar(GameObject car, Vector3 Position, float YAngle)
    {
        PlayerCar = new CarPlayer();

        _car = Object.Instantiate(car);

        _car.transform.position = Position;
        _car.transform.rotation = Quaternion.Euler(0f, YAngle, 0f);

        PlayerCar.Initialize(_car);

        return _car;
       
    }

  
}
