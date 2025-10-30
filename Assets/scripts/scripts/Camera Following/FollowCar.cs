using System.Collections;
using UnityEngine;

public class FollowCar : MonoBehaviour
{
    ICameraSwitcher BirdEyeCam;
    ICameraSwitcher BonnetCam;
    ICameraSwitcher ChaseCam;
    ICameraSwitcher DroneCam;

    ICameraSwitcher CurrentSwitchedCam;
    ICommonCarEntity commonCarEntity;

    [SerializeField] private Camera Camera;
    private Transform FollowAnchor;
    private GameObject PlayerCar;
    private Rigidbody carRb;

    private bool CameraSwitched;
    private int MaxSwitches;
    private int Index;
    private bool cameraActive = false;

    public void Initialize()
    {
        StartCoroutine(FindPlayerCar());
        MaxSwitches = 4;

        // Subscribe to race start
        GameStartEndEvent.OnRaceStarted += OnRaceStarted;
    }

    IEnumerator FindPlayerCar()
    {
        yield return new WaitUntil(() =>
        {
            var car = GameObject.FindGameObjectWithTag("Player");
            return car != null && car.transform.GetChild(0) != null;
        });

        PlayerCar = GameObject.FindGameObjectWithTag("Player");
        FollowAnchor = PlayerCar.transform.GetChild(0);
        carRb = PlayerCar.GetComponent<Rigidbody>();
        commonCarEntity = PlayerCar.GetComponent<CommonEntity>();

        // Create and initialize cameras
        BirdEyeCam = new BirdEyeCamera();
        ChaseCam = new ChaseCamera();
        BonnetCam = new BonnetCamera();
        DroneCam = new DroneCamera();

        BirdEyeCam.Initialize(Camera.transform , FollowAnchor);
        ChaseCam.Initialize(Camera.transform, FollowAnchor);
        BonnetCam.Initialize(Camera.transform, FollowAnchor);
        DroneCam.Initialize(Camera.transform, FollowAnchor);

        CurrentSwitchedCam = ChaseCam;
    }

    void OnRaceStarted()
    {
        // Delay allows Rigidbody to stabilize before camera starts
        StartCoroutine(ActivateCameraAfterDelay(0.25f));
    }

    IEnumerator ActivateCameraAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        cameraActive = true;

        if (carRb != null && FollowAnchor != null)
        {
            // Initialize ChaseCam with spawn direction
            if (CurrentSwitchedCam is ChaseCamera chase)
            {
                chase.InitializePosition(Camera.transform, FollowAnchor);
            }
        }

       
    }

    private void Update()
    {
        if (Input.GetKeyDown(ServiceLocator.Instance.GetService<SettingManager>().M_Control.ControlSettings[ControlSettingKey.SwitchCam]))
        {
            Index = (Index + 1) % MaxSwitches;
            CameraSwitched = true;
        }
    }

    private void LateUpdate()
    {
        if (!cameraActive || FollowAnchor == null || carRb == null || commonCarEntity == null)
            return;

        if (CameraSwitched)
        {
            CameraSwitched = false;
            switch (Index)
            {
                case 0: CurrentSwitchedCam = ChaseCam; break;
                case 1: CurrentSwitchedCam = BonnetCam; break;
                case 2: CurrentSwitchedCam = BirdEyeCam; break;
                case 3: CurrentSwitchedCam = DroneCam; break;
            }
        }

        CurrentSwitchedCam.FollowCar(Camera.transform, FollowAnchor, carRb, commonCarEntity.Speed);
    }

    private void OnDestroy()
    {
        GameStartEndEvent.OnRaceStarted -= OnRaceStarted;
    }
}
