using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICarFactory 
{
    GameObject CreateCar(GameObject car , Vector3 Position , float YAngle);
}
