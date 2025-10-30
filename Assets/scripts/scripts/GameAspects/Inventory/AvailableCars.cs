using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AvailableCars : MonoBehaviour
{
    private AsyncOperationHandle<IList<GameObject>> OperationHandle;

    private Vector3 SpawnPosition;

    private Vector3 CurrentCamPos;

    private float TargetZCamPos;

    private float ZDistanceBetweenCars;

    private int CarsIndex = 0;

    private int lastCarsIndex = -1;

    private float FinalZPosOfCam;

    private float InitialZPosOfCam;

    [SerializeField] private Camera Camera;

    private InventoryModel Model;

    private InventoryUI View;

    private List<CarInfo> AllCarInfos;

    private List<CarInfo> SelectedTypeCarInfos;

    private List<string> Labels;

    private GameObject CarModel;

    private CarType carType;

    [HideInInspector] public SelectCarButton selectButton;
    void Start()
    {
        selectButton = FindFirstObjectByType<SelectCarButton>();

        Model = new InventoryModel();

        View = FindObjectOfType<InventoryUI>();

        SpawnPosition = Vector3.zero;

        ZDistanceBetweenCars = 5f;

        InitialZPosOfCam = Camera.transform.position.z;

        AllCarInfos = ServiceLocator.Instance.GetService<InventoryManager>().AvailableCars;

        SelectedTypeCarInfos = new();

        Labels = new();

        

        InitializeListOfSelectedTypeCars();

        StartCoroutine(LoadCars());

        TargetZCamPos = Camera.transform.position.z;

        Model.OnDataChange += UpdateView;

    }


    void InitializeListOfSelectedTypeCars()
    {
        if (ServiceLocator.Instance.GetService<LevelManager>().selectedLevel.raceType == RaceType.DriftRace) carType = CarType.Drift;
        else carType = CarType.Race;

       SelectedTypeCarInfos = AllCarInfos.Where(car => car.CarType == carType).OrderBy(car => car.CarID).ToList();
    }

  

    IEnumerator LoadCars()
    {
       foreach(CarInfo car in SelectedTypeCarInfos)
        {
            Labels.Clear();

            Labels.Add(car.CarType.ToString());
            Labels.Add(car.CarID.ToString());

            OperationHandle = Addressables.LoadAssetsAsync<GameObject>(Labels, InstantiationOperation, Addressables.MergeMode.Intersection);

            yield return OperationHandle;   
        }
    }
    void InstantiationOperation(GameObject obj)
    {
        FinalZPosOfCam = InitialZPosOfCam + SpawnPosition.z;

        CarModel = Instantiate(obj);

        SpawnPosition += Vector3.forward * ZDistanceBetweenCars;

        CarModel.transform.localPosition = SpawnPosition;
        CarModel.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);



    }
    void Update()
    {
        CameraMoveToShowSelectedCar();
    }

    void CameraMoveToShowSelectedCar()
    {
        CurrentCamPos = Camera.transform.position;


        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (CarsIndex < SelectedTypeCarInfos.Count - 1) CarsIndex++;
            TargetZCamPos = Mathf.Clamp(CurrentCamPos.z + ZDistanceBetweenCars, InitialZPosOfCam, FinalZPosOfCam);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (CarsIndex > 0) CarsIndex--;
            TargetZCamPos = Mathf.Clamp(CurrentCamPos.z - ZDistanceBetweenCars, InitialZPosOfCam, FinalZPosOfCam);
        }

        if (lastCarsIndex != CarsIndex) { UpdateModelData(SelectedTypeCarInfos[CarsIndex]); lastCarsIndex = CarsIndex; }
        selectButton.carInfo = SelectedTypeCarInfos[CarsIndex];

        CurrentCamPos.z = Mathf.Lerp(CurrentCamPos.z, TargetZCamPos, Time.deltaTime * 10f);
        Camera.transform.position = CurrentCamPos;


    }

    void UpdateView(InventoryModel Model)
    {
        View.UpdateNameText(Model.Name);
        View.UpdateMaxSpeedText(Model.MaxSpeed);
        View.UpdateTorqueText(Model.Torque);
        View.UpdateCorneringText(Model.Cornering);
        View.UpdateAccelerationText(Model.Acceleration);
    }

    void UpdateModelData(CarInfo info)
    {

        Model.SetMaxSpeed(info.MaxSpeed);
        Model.SetName(info.CarName);
        Model.SetAcceleration(info.Acceleration);
        Model.SetCornering(info.Cornering);
        Model.SetTorque(info.Torque);

    }

   
    private void OnDisable()
    {
        if (OperationHandle.IsValid()) OperationHandle.Release();
    }
}

