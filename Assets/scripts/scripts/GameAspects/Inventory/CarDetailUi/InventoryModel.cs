using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryModel
{
    private int _MaxSpeed;
    private int _Torque;
    private string _Name;
    private float _Acceleration;
    private float _Cornering;

    public int MaxSpeed => _MaxSpeed;
    public int Torque => _Torque;
    public string Name => _Name;
    public float Acceleration => _Acceleration;
    public float Cornering => _Cornering;

    public event Action<InventoryModel> OnDataChange;

    public void SetMaxSpeed(int value) { _MaxSpeed = value; NotifyDataChange(); }
    public void SetTorque(int value) { _Torque = value; NotifyDataChange(); }
    public void SetName(string value) { _Name = value; NotifyDataChange(); }
    public void SetAcceleration(float value) { _Acceleration = value; NotifyDataChange(); }
    public void SetCornering(float value) { _Cornering = value; NotifyDataChange(); }

    public void NotifyDataChange()
    {
        OnDataChange?.Invoke(this);
    }

     

}
