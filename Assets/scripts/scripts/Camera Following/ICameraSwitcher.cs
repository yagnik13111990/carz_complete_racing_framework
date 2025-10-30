using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICameraSwitcher 
{
    void Initialize(Transform Camera , Transform PlayerInitialPosition);
    void FollowCar(Transform Camera , Transform Car , Rigidbody carRb , float speed );

    
}
