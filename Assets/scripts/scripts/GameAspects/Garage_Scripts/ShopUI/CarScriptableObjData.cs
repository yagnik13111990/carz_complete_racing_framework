using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Car Object" , menuName = "Car Shop Data" , order = 1)]
public class CarScriptableObjData : ScriptableObject
{
    public int CarID;

    public string CarName;

    public int Price;

    public int Maxspeed;

    public float Acceleration;

    public int Cornering;

    public int Torque;

    public CarType CarType;
}
