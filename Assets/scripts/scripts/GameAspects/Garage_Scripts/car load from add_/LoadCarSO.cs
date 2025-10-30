using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LoadCarSO : MonoBehaviour
{
    private List<CarScriptableObjData> _LoadedData = new List<CarScriptableObjData>();

    public  List<CarScriptableObjData> LoadedData{ get { return _LoadedData; } }
    private AsyncOperationHandle<IList<CarScriptableObjData>> OperationHandle;


    private IObserverOfSelectedCar observerOfSelectedCar;
    private IObserverOfSelectedCar carModel;
    private IObserverOfSelectedCar purchaseCar;
    // Start is called before the first frame update
    void Start()
    {
        observerOfSelectedCar = FindObjectOfType<ObserverOfSelectedCar>();
        carModel = FindObjectOfType<LoadCarModel>();
        purchaseCar = FindObjectOfType<PurchaseCar>();
        OperationHandle = Addressables.LoadAssetsAsync<CarScriptableObjData>("car scriptable obj" , null);
       
        OperationHandle.Completed += OnLoadingCarSO;


    }

   

    void OnLoadingCarSO(AsyncOperationHandle<IList<CarScriptableObjData>> handle)
    {
       
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            _LoadedData.AddRange(handle.Result);

            _LoadedData.Sort(CompareCarID);
            for (int i = 0; i < CollectionOfObservables.Observables.Count; i++)
            {
                CollectionOfObservables.Observables[i].LetSubscribe(carModel);
                CollectionOfObservables.Observables[i].LetSubscribe(observerOfSelectedCar);
                CollectionOfObservables.Observables[i].LetSubscribe(purchaseCar);
                CollectionOfObservables.Observables[i].SetCarDetail(_LoadedData[i]);
            }
        }
      
    }

    int CompareCarID(CarScriptableObjData A , CarScriptableObjData B)
    {
        if (A.CarID > B.CarID) return 1;
        else return -1;
    }
   
    private void OnDisable()
    {
        for (int i = 0; i < CollectionOfObservables.Observables.Count; i++)
        {
            CollectionOfObservables.Observables[i].RemoveSubscriptionOf(observerOfSelectedCar);

        }

       
    
        if (OperationHandle.IsValid()) OperationHandle.Release();
    
    }

}


