

using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.VisualScripting;
using UnityEngine;

public class AIEntity : MonoBehaviour
{
    private Vector3 MidForwardPosition;
    private Vector3 RightForwardPosition;
    private Vector3 LeftForwardPosition;
    private Vector3 LeftCrossDirection;
    private Vector3 RightCrossDirection;
    private Vector3 Position;

    [Header("Raycast Properties")]
    [SerializeField] private float frontRayCastPos = 1.63f;
    [SerializeField] private float sideRayCastPos = 0.56f;
    [SerializeField] private float upRayCastPos = 0.5f;
    [SerializeField] private float RayLength = 5.75f;
    [SerializeField] private float RayAngle = 17.1f;

    [Header("Reverse Logic")]
    [SerializeField] private float stuckThreshold = 5f;
    [SerializeField] private float stuckTime = 2f;      
    [SerializeField] private float reverseDuration = 1.5f;

    private RaycastHit hit;
   
    private IDriveHandle driveHandle;
    private ICommonCarEntity commonCarEntity;
    Entity aiEntity;
    EntityManager entityManager;

    private float stuckTimer = 0f;
    private float reverseTimer = 0f;
    private bool isReversing = false;
    private string Tag = "RoadBarier";
   // private Rigidbody rb;
    int count = 0;

  
    void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        commonCarEntity = GetComponent<CommonEntity>();
        driveHandle = GetComponent<DriveHandle>();

        aiEntity = commonCarEntity.CarEntity;

       
        if (!entityManager.HasComponent<AICar>(aiEntity))
        {
            entityManager.AddComponentData(aiEntity, new AICar());
        }
    }

   
    private void FixedUpdate()
    {
        commonCarEntity.BrakeInput = 0f;
        commonCarEntity.Throttle = 1f;

        Position = transform.position;
        LeftCrossDirection = Quaternion.AngleAxis(-RayAngle, transform.up) * transform.forward;
        RightCrossDirection = Quaternion.AngleAxis(RayAngle, transform.up) * transform.forward;

        if (count == 5) commonCarEntity.TurnMultiplier = 0;

        
        if(Time.frameCount % 3 == 0)
        {
            MidCast();
            LeftCast();
            RightCast();
        }


        HandleStuck();


    }
    void MidCast()
    {
        MidForwardPosition = Position + transform.forward * frontRayCastPos + transform.up * upRayCastPos;

        if (Physics.Raycast(MidForwardPosition, transform.forward, out hit, RayLength + 1.4f))
        {

            Debug.DrawLine(MidForwardPosition, hit.point, Color.white);
            if (hit.collider.tag == Tag && hit.distance > (0.3f * (RayLength + 1.4f))) return;
            else
            {
                count++;

                // brake if obstacle ahead
                commonCarEntity.Throttle = 0f;
                if(driveHandle.RB.velocity.magnitude > 5f) commonCarEntity.BrakeInput = 1.5f;

                // if no steering yet, decide based on hit normal
                if (commonCarEntity.TurnMultiplier == 0)
                    commonCarEntity.TurnMultiplier = (hit.normal.x < 0) ? -1f : 1f;
            }
        }
    }

    void LeftCast()
    {
        LeftForwardPosition = Position + transform.forward * frontRayCastPos + transform.up * upRayCastPos - transform.right * sideRayCastPos;

        if (Physics.Raycast(LeftForwardPosition, transform.forward, out hit, RayLength))
        {
            Debug.DrawLine(LeftForwardPosition, hit.point, Color.green);
            if (hit.collider.tag == Tag && hit.distance > (0.3f * RayLength)) return;
           
            else { count++; commonCarEntity.TurnMultiplier -= 1f; }// steer right

            if (driveHandle.RB.velocity.magnitude > 5f) commonCarEntity.BrakeInput = 0.5f;
        }

        if (Physics.Raycast(LeftForwardPosition, LeftCrossDirection, out hit, RayLength))
        {
            Debug.DrawLine(LeftForwardPosition, hit.point, Color.yellow);
            if (hit.collider.tag == Tag && hit.distance > (0.3f * RayLength)) return;
            else { count++; commonCarEntity.TurnMultiplier -= 0.5f; }// steer right slightly

            if (driveHandle.RB.velocity.magnitude > 5f) commonCarEntity.BrakeInput = 0.5f;
        }
    }

    void RightCast()
    {
        RightForwardPosition = Position + transform.forward * frontRayCastPos + transform.up * upRayCastPos + transform.right * sideRayCastPos;

        if (Physics.Raycast(RightForwardPosition, transform.forward, out hit, RayLength))
        {
            Debug.DrawLine(RightForwardPosition, hit.point, Color.magenta);
            if (hit.collider.tag == Tag && hit.distance > (0.3f * RayLength)) return;
            else { count++; commonCarEntity.TurnMultiplier += 1f; } // steer left

            if (driveHandle.RB.velocity.magnitude > 5f) commonCarEntity.BrakeInput = 0.5f;
        }

        if (Physics.Raycast(RightForwardPosition, RightCrossDirection, out hit, RayLength))
        {
            Debug.DrawLine(RightForwardPosition, hit.point, Color.blue);
            if (hit.collider.tag == Tag && hit.distance > (0.3f * RayLength)) return;
            else { count++; commonCarEntity.TurnMultiplier += 0.5f; }// steer left slightly

            if (driveHandle.RB.velocity.magnitude > 5f) commonCarEntity.BrakeInput = 0.5f;
        }
    }

   
        


    

    void HandleStuck()
    {
        float speed = driveHandle.RB.velocity.magnitude;

        if (!isReversing && speed < stuckThreshold)
        {
            stuckTimer += Time.deltaTime;
            if (stuckTimer > stuckTime)
            {
                isReversing = true;
                reverseTimer = 0f;
            }
        }
        else
        {
            stuckTimer = 0f;
        }

        if (isReversing)
        {
            commonCarEntity.Throttle = -1f;
            commonCarEntity.BrakeInput = 0f;

            // flip steering when backing up
            commonCarEntity.TurnMultiplier *= -1f;

             reverseTimer += Time.deltaTime;

            if (reverseTimer > reverseDuration)
            {
                isReversing = false;
            }
        }
    }

  
   
}
