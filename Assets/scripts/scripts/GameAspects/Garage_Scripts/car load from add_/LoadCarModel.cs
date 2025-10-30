using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LoadCarModel : MonoBehaviour , IObserverOfSelectedCar
{
   [SerializeField] private float RotationSpeed;
    private Quaternion Rotation;

    private AsyncOperationHandle<IList<GameObject>> OperationHandle;

    [SerializeField] private GameObject CarShowCase;

    private GameObject CarModel;
 
    private List<string> Labels = new();

    private int? LastCarID;

   
    void SpawnModel((string type , string id) carInfo)
    {
        ReleaseCurrentCar();
        Labels.Clear();
        Labels.Add(carInfo.id);
        Labels.Add(carInfo.type);

        OperationHandle = Addressables.LoadAssetsAsync<GameObject>(Labels, InstantiateLoadedCar, Addressables.MergeMode.Intersection);

        LastCarID = int.Parse(carInfo.id);
    }

    void InstantiateLoadedCar(GameObject obj)
    {
         CarModel = Instantiate(obj ,  CarShowCase.transform);
         CarModel.transform.localPosition = Vector3.zero;
         CarModel.transform.localRotation = Quaternion.identity;
           
    }
    public void UpdateStates(CarScriptableObjData data)
    {
        int NewCarID = data.CarID;

        if (LastCarID == NewCarID) return;
           
        SpawnModel((data.CarType.ToString(), data.CarID.ToString()));
        
    }

    void ReleaseCurrentCar()
    {
        if(CarModel != null)
        {
            Destroy(CarModel);
            CarModel = null;
        }

        if (OperationHandle.IsValid())
        {
            Addressables.Release(OperationHandle);
            
        }

        LastCarID = null;
    }
   
    private void Update()
    {
        CarShowCase.transform.Rotate(0, RotationSpeed * Time.deltaTime, 0, Space.Self);
    
    }

    private void OnDisable()
    {
        ReleaseCurrentCar();
    }
}
