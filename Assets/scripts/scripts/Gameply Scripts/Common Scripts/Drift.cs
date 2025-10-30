using UnityEngine;

public class Drift : MonoBehaviour
{
    [SerializeField] private Transform CarNormal;
    [SerializeField] private Transform CarModel;
    [SerializeField] private Rigidbody Sphere;

    [Header("Car Property")]
    public float MaximumSpeed;

    [Header("parameters ")]
    [SerializeField] private float ReverseAccel;
    [SerializeField] private float Steer;
    [SerializeField] private float AutoDriftThreshHold;
    [SerializeField] private LayerMask LayerMask;
    [SerializeField] private float LongRay;

    public float rotationVelocity;

    public Transform FL, FR;
    public Transform RL, RR;

    public float Acceleration { get; set; }
    public float BaseAcceleration { get; private set; }

    float Speed;
    float Rotation;
    float CurrentRotation;
    bool isDrifting;
    float DriftDirection;
    float SteerAmount;
    float SteerDirection;

    float smoothThrottle;
    float smoothTurn;
    float speedMagnitude;

    public float Throttle, TurnMultiplier;
    public float CurrentSpeed { get; private set; }
    public float SpeedInUnit { get; private set; }

    // --- Brake Torque System ---
    [Header("Brake Torque System")]
    [SerializeField] private float maxBrakeStrength = 25f; // how strong the braking deceleration is
    [SerializeField] private float brakeSmoothness = 5f;   // how fast brake torque reacts
    private float brakeTorque;           // current brake torque value (0–1)
    private float targetBrakeTorque;     // target brake torque set externally

    private void Start()
    {
        BaseAcceleration = (20 * MaximumSpeed) / 280f;
        Acceleration = BaseAcceleration;
        Sphere.interpolation = RigidbodyInterpolation.Interpolate;
    }

    public void InitializeDamper(float damper)
    {
        MaximumSpeed *= damper;
    }

    private void Update()
    {
        speedMagnitude = Sphere.velocity.magnitude;
        SpeedInUnit = (280f * CurrentSpeed) / 20f;
        transform.position = Sphere.position - new Vector3(0, 0.4f, 0);

        smoothThrottle = Mathf.MoveTowards(smoothThrottle, Throttle, Time.deltaTime * 2f);

        float targetSpeed = 0f;

        if (smoothThrottle > 0f)
            targetSpeed = Acceleration * smoothThrottle;
        else if (smoothThrottle < 0f)
            targetSpeed = ReverseAccel * smoothThrottle;

        Speed = targetSpeed;

        smoothTurn = Mathf.Lerp(smoothTurn, TurnMultiplier, Time.deltaTime * 3f);
        float targetSteer = Mathf.Clamp(smoothTurn, -1f, 1f);

        float speedFactor = Mathf.Clamp01(Sphere.velocity.magnitude / MaximumSpeed);
        float adaptiveSteerResponse = Mathf.Lerp(6f, 3f, speedFactor);

        SteerDirection = Mathf.Lerp(SteerDirection, targetSteer, Time.deltaTime * adaptiveSteerResponse);
        SteerAmount = Mathf.Abs(SteerDirection);

        float targetDrift = Mathf.Abs(targetSteer) > 0.1f ? Mathf.Sign(targetSteer) : 0f;
        DriftDirection = targetDrift;

        Steering(SteerDirection, SteerAmount);
        HandleDrift(SteerAmount);

        CurrentSpeed = Mathf.Lerp(CurrentSpeed, Speed, Time.deltaTime * 12f);
        CurrentRotation = Mathf.Lerp(CurrentRotation, Rotation, Time.deltaTime * 4f);
        Rotation = 0;

        float currentVelocity = Sphere.velocity.magnitude;

        // --- Rotation disabled when car is nearly stopped ---
        if (currentVelocity > 0.1f)
        {
            CarModel.localRotation = Quaternion.Slerp(CarModel.localRotation, Quaternion.identity, Time.deltaTime * 5f);
        }
        else
        {
            if (!isDrifting)
            {
                Quaternion targetSteerRot = Quaternion.Euler(0, SteerDirection * 15f, 0);
                CarModel.localRotation = Quaternion.Slerp(CarModel.localRotation, targetSteerRot, Time.deltaTime * 5f);
            }
            else if (DriftDirection != 0)
            {
                float control = DriftDirection > 0.1f
                    ? Remap(SteerDirection, -1, 1, 0.5f, 2f)
                    : Remap(SteerDirection, -1, 1, 2f, 0.5f);

                float driftAngle = control * 15f * SteerDirection;
                Quaternion targetRot = Quaternion.Euler(0f, driftAngle, 0f);

                CarModel.parent.localRotation = Quaternion.RotateTowards(
                    CarModel.parent.localRotation,
                    targetRot,
                    Mathf.Abs(rotationVelocity) * Time.deltaTime);
            }
        }

        // Wheel spin (based on velocity)
        float wheelSpin = speedMagnitude * 8f * Time.time;
        Quaternion frontTarget = Quaternion.Euler(wheelSpin, SteerDirection * Steer, 0);
        Quaternion rearTarget = Quaternion.Euler(wheelSpin, 0, 0);

        FL.localRotation = Quaternion.Slerp(FL.localRotation, frontTarget, Time.deltaTime * 10f);
        FR.localRotation = Quaternion.Slerp(FR.localRotation, frontTarget, Time.deltaTime * 10f);
        RL.localRotation = Quaternion.Slerp(RL.localRotation, rearTarget, Time.deltaTime * 10f);
        RR.localRotation = Quaternion.Slerp(RR.localRotation, rearTarget, Time.deltaTime * 10f);

        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;

        if (Time.frameCount % 2 == 0)
        {
            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hitNear, LongRay, LayerMask))
            {
                if (Vector3.Angle(CarNormal.up, hitNear.normal) > 0.1f)
                {
                    Vector3 forward = Vector3.ProjectOnPlane(transform.forward, hitNear.normal);
                    Quaternion targetRotation = Quaternion.LookRotation(forward, hitNear.normal);
                    CarNormal.rotation = Quaternion.Slerp(CarNormal.rotation, targetRotation, Time.deltaTime * 3f);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        Vector3 forward = new Vector3(CarModel.forward.x, 0f, CarModel.forward.z).normalized;
        float currentVelocity = Sphere.velocity.magnitude;
     
        // Keep drift / acceleration behavior
        if (!isDrifting)
            Sphere.AddForce(forward * CurrentSpeed, ForceMode.Acceleration);
        else
            Sphere.AddForce(forward * CurrentSpeed, ForceMode.Acceleration);

        // --- Apply brake torque if active ---
        brakeTorque = Mathf.Lerp(brakeTorque, targetBrakeTorque, Time.fixedDeltaTime * brakeSmoothness);

        if (brakeTorque > 0.01f)
        {
            Vector3 brakeForce = -Sphere.velocity.normalized * brakeTorque * maxBrakeStrength;
            Sphere.AddForce(brakeForce, ForceMode.Acceleration);
        }

        // Apply gravity
        Sphere.AddForce(-transform.up * 10f, ForceMode.Force);

        // Rotation only when moving
        if (currentVelocity > 0.1f)
            transform.Rotate(0, CurrentRotation * Time.fixedDeltaTime * 6f, 0, Space.World);
    }

    private void HandleDrift(float Amount)
    {
        isDrifting = Sphere.velocity.magnitude > AutoDriftThreshHold && Mathf.Abs(Amount) > 0.1f;

        if (isDrifting)
        {
            float control = DriftDirection > 0.1f
                ? Remap(SteerDirection, -1, 1, 0, 2)
                : Remap(SteerDirection, -1, 1, 2, 0);

            Steering(DriftDirection, control);
        }
    }

    private void Steering(float Direction, float Amount)
    {
        Rotation = Steer * Direction * Amount;
    }

    private float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    

    public void SetThrottle(float value) => Throttle = value;
    public void SetTurn(float value) => TurnMultiplier = value;

   
    public void ApplyBrakeTorque(float amount)
    {
        if(speedMagnitude > 10f)
        targetBrakeTorque = Mathf.Clamp01(amount);
    }

    
    public void ReleaseBrake()
    {
        targetBrakeTorque = 0f;
    }
}
