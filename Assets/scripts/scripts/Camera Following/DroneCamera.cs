using UnityEngine;

public class DroneCamera : ICameraSwitcher
{
    private Vector3 positionOffset;
    private float baseFollowSpeed = 5f;
    private float maxFollowSpeed = 10f;
    private float smoothVelocity;
    private float smoothSpeed;

    public void Initialize(Transform Camera , Transform PlayerInitialPosition)
    {
        // Based on your manually placed camera relative to the car anchor
        positionOffset = new Vector3(-16.49f, 12.56f, -12.49f);
    }

    public void FollowCar(Transform Camera, Transform Car, Rigidbody carRb, float speed)
    {
        // --- Smooth speed-based follow ---
        smoothSpeed = Mathf.SmoothDamp(smoothSpeed, speed, ref smoothVelocity, 0.4f);
        float followSpeed = Mathf.Lerp(baseFollowSpeed, maxFollowSpeed, smoothSpeed / 20f);

        // --- Stable offset relative to ANCHOR (already child of player) ---
        Vector3 targetPosition = carRb.position + Car.TransformDirection(positionOffset);

        // Smooth movement
        Camera.position = Vector3.Lerp(Camera.position, targetPosition, Time.deltaTime * followSpeed);

        // --- Look slightly above anchor to give cinematic view ---
        Vector3 lookTarget = Car.position + Vector3.up * 2f;
        Quaternion targetRotation = Quaternion.LookRotation(lookTarget - Camera.position, Vector3.up);

        Camera.LookAt(Car.transform);
    }
}
