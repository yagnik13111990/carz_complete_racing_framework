using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInfoBuilder 
{
    private CarInfo _carInfo = new CarInfo();

   

    public CarInfoBuilder SetCarType (CarType carType)
    {
        _carInfo.CarType = carType;
        return this;
    }

    public CarInfoBuilder SetCarID(int carID)
    {
        _carInfo.CarID = carID;
        return this;
    }

    public CarInfoBuilder SetMaximumSpeed(int max)
    {
        _carInfo.MaxSpeed = max;
        return this;
    }

    public CarInfoBuilder SetCarName(string name)
    {
        _carInfo.CarName = name;
        return this;
    }
    public CarInfoBuilder SetAcceleration(float acc)
    {
        _carInfo.Acceleration = acc;
        return this;
    }

    public CarInfoBuilder SetTorque(int tor)
    {
        _carInfo.Torque = tor;
        return this;
    }

    public CarInfoBuilder SetCornering(int corner)
    {
        _carInfo.Cornering = corner;
        return this;
    }

    public CarInfo Build()
    {
        return _carInfo;
    }
}
