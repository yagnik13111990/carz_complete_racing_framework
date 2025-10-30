using UnityEngine;

public class ChaseCamera : ICameraSwitcher
{
    private float baseDistance = 8f;
    private float maxDistance = 12f;
    private float currentDistance;
    private Vector3 lastStableDir = Vector3.forward;
    private bool initializedPosition = false;

    public void Initialize(Transform Camera, Transform PlayerInitialPosition)
    {
        currentDistance = baseDistance;
        Camera.position = PlayerInitialPosition.position - PlayerInitialPosition.forward * baseDistance + PlayerInitialPosition.up * 1.2f;
        Camera.LookAt(PlayerInitialPosition.position);
        initializedPosition = false;
    }

    public void InitializePosition(Transform Camera, Transform Car)
    {
        lastStableDir = Car.forward;
        Vector3 offset = Car.TransformPoint(new Vector3(0f, 0.7f, 0f));
        Camera.position = Car.position - lastStableDir * baseDistance + (offset - Car.position);
        Camera.rotation = Quaternion.LookRotation(Car.position + Vector3.up * 0.5f - Camera.position, Vector3.up);
        initializedPosition = true;
    }

    public void FollowCar(Transform Camera, Transform Car, Rigidbody carRb, float speed)
    {
        if (!initializedPosition)
            InitializePosition(Camera, Car);

        
        float speedRatio = Mathf.Clamp01(speed / 20f);
        currentDistance = Mathf.Lerp(baseDistance, maxDistance, speedRatio);

       
        Vector3 flatVel = new Vector3(carRb.velocity.x, 0, carRb.velocity.z);
        if (flatVel.sqrMagnitude > 0.1f)
            lastStableDir = Vector3.Slerp(lastStableDir, flatVel.normalized, 5f * Time.deltaTime);

        Vector3 offsetPos = Car.TransformPoint(new Vector3(0f, 1.2f, 0f));
        Vector3 targetPos = Car.position - lastStableDir * currentDistance + (offsetPos - Car.position);

        
        float moveSpeed = Mathf.Lerp(45f, 89f, speedRatio);
        Camera.position = Vector3.MoveTowards(Camera.position, targetPos, moveSpeed * Time.deltaTime);

       
        Quaternion targetRot = Quaternion.LookRotation(Car.position + Vector3.up * 0.5f - Camera.position, Vector3.up);
        float rotationSpeed = 180f; 
        Camera.rotation = Quaternion.RotateTowards(Camera.rotation, targetRot, rotationSpeed * Time.deltaTime);
    }
}
