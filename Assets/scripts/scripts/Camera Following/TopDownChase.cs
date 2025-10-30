using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownChase :ICameraSwitcher
{
    private Vector3 Offset;

    public void Initialize(Transform Camera , Transform PlayerInitialPosition)
    {
       
        Offset = new Vector3(0f, 3f, -7f);
    }

    public void FollowCar(Transform Camera, Transform Car, Rigidbody carRb, float speed)
    {
        
        Vector3 targetPosition = Car.TransformPoint(Offset);

        
        Camera.position = targetPosition;

       
        Camera.LookAt(Car.position + Vector3.up * 1.2f);
    }
}
