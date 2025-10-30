using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarFactory : ICarFactory
{
    private GameObject _car;

    private ICar AiCar;
    public GameObject CreateCar(GameObject car, Vector3 Position, float YAngle)
    {
        AiCar = new CarAI();

        _car = Object.Instantiate(car);

        _car.transform.position = Position;
        _car.transform.rotation = Quaternion.Euler(0f,YAngle , 0f);

        AiCar.Initialize(_car);

        return _car;
       
    }

   
}
