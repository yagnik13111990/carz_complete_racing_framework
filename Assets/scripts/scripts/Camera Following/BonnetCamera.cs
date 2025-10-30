using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BonnetCamera : ICameraSwitcher
{
    private float LerpTime ;

    private bool isReversing;

    private float TargetRotation;

    private Vector3 TargetPos;

    private Quaternion LookTarget;

    private Vector3 Direction;

    private Vector3 Offset;


    public void Initialize(Transform Camera , Transform PlayerInitialPosition)
    {
        LerpTime = 7f;

        Offset = new Vector3(0f, 1.06f, -1.020005f);
    }

    public void FollowCar(Transform Camera, Transform Car, Rigidbody carRb , float speed )
    {
        TargetPos = Car.TransformPoint(Car.up * 1f );

        Camera.position = TargetPos;

       
        if (carRb.velocity.sqrMagnitude > 0.1f)
        {
            float dot = Vector3.Dot(carRb.velocity.normalized, Car.forward);

           
            if (!isReversing && dot < -0.2f)
                isReversing = true;

            else if (isReversing && dot > 0.5f)
                isReversing = false;
        }

       
        if (isReversing)
            LookTarget = Quaternion.LookRotation(-Car.forward, Vector3.up);  

        else
            LookTarget = Quaternion.LookRotation(Car.forward, Vector3.up);

       
        Camera.rotation = Quaternion.Slerp(Camera.rotation, LookTarget, Time.deltaTime * LerpTime);
    }
}
