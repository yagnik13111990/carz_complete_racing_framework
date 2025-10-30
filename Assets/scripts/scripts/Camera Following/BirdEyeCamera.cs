using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdEyeCamera : ICameraSwitcher
{
    private Vector3 MovementDir;

    private Vector3 TargetPos;

    private Vector3 LookTarget;

    private float HeightOfCam ;

    private float LookAheadDistance;

    private float LerpTime;

    private float BaseSpeed;

    private float MaxSpeed;

    private float FollowSpeed;

    private float SmoothSpeed;

    private float SmoothingTime;

    private float SmoothVelocity;
    public void Initialize(Transform Camera , Transform PlayerInitialPosition)
    {
        HeightOfCam = 30f;

        LookAheadDistance = 2f;

        LerpTime = 2f;

       
    }
    public void FollowCar(Transform Camera, Transform Car, Rigidbody carRb, float speed)
    {

       
        MovementDir = carRb.velocity.normalized;


        if (speed < 10f) FollowSpeed = 5f;

        else
        {
            if (speed > SmoothSpeed)
            {

                SmoothingTime = 1.2f; // fast reaction
            }
            else
            {

                SmoothingTime = 2.0f; // slower
            }

            SmoothSpeed = Mathf.SmoothDamp(SmoothSpeed, speed, ref SmoothVelocity, SmoothingTime);

            FollowSpeed = Mathf.Lerp(BaseSpeed, MaxSpeed, SmoothSpeed);
        }

        TargetPos = Car.position + Vector3.up * HeightOfCam;

        Camera.position = Vector3.Lerp(Camera.position, TargetPos, FollowSpeed * Time.deltaTime);


        
        LookTarget = Car.position + Car.forward * LookAheadDistance;

        Camera.rotation = Quaternion.Lerp(

            Camera.rotation,
            Quaternion.LookRotation(LookTarget - Camera.position, Vector3.up),
            Time.deltaTime * LerpTime

        );
    }

   
}
