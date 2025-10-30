using Unity.Entities.UniversalDelegates;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class DriftEntity : MonoBehaviour 
{
    private float handBrakeFrictionMultiplier;

    private WheelFrictionCurve forwardFriction, sidewaysFriction;

    IDriveHandle driveHandle;
    ICommonCarEntity carEntity;

    private float Speed;

    private void Start()
    {
        carEntity = GetComponent<CommonEntity>();
        driveHandle = GetComponent<DriveHandle>();

        handBrakeFrictionMultiplier = 2f;

    }
    
    private void FixedUpdate()
    {
        Speed = carEntity.Speed;

        ApplyTractionAdjustments();

    }
    private void ApplyTractionAdjustments()
    {
        float driftSmoothFactor = 0.7f * Time.deltaTime;

        if (driveHandle.BrakeInput == 1)
        {
            sidewaysFriction = driveHandle.SidewayFrictions[0];
            forwardFriction = driveHandle.ForwardFrictions[0];

            float velocity = 0;
            sidewaysFriction.extremumValue =
            sidewaysFriction.asymptoteValue =
            forwardFriction.extremumValue =
            forwardFriction.asymptoteValue =

                Mathf.SmoothDamp(forwardFriction.asymptoteValue,
                                 driveHandle.DriftFactor * handBrakeFrictionMultiplier,
                                 ref velocity,
                                 driftSmoothFactor);

            for (int i = 0; i < 4; i++)
            {
                driveHandle.SidewayFrictions[i] = sidewaysFriction;
                driveHandle.ForwardFrictions[i] = forwardFriction;
            }

            sidewaysFriction.extremumValue =
            sidewaysFriction.asymptoteValue =
            forwardFriction.extremumValue =
            forwardFriction.asymptoteValue = 1.1f;


            for (int i = 2; i < 4; i++)
            {
                driveHandle.SidewayFrictions[i] = sidewaysFriction;
                driveHandle.ForwardFrictions[i] = forwardFriction;
            }

            driveHandle.RB.AddForce(driveHandle.RB.transform.forward * (Speed / 400) * 40000);
        }

        else
        {
            forwardFriction = driveHandle.ForwardFrictions[0];
            sidewaysFriction = driveHandle.SidewayFrictions[0];

            float baseGrip = ((Speed * handBrakeFrictionMultiplier) / 300) + 1;

            forwardFriction.extremumValue = forwardFriction.asymptoteValue =
            sidewaysFriction.extremumValue = sidewaysFriction.asymptoteValue = baseGrip;

            for (int i = 0; i < 4; i++)
            {
                driveHandle.SidewayFrictions[i] = sidewaysFriction;
                driveHandle.ForwardFrictions[i] = forwardFriction;
            }
        }


       
    }
}
