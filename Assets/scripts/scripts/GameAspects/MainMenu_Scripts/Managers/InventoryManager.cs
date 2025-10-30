using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager
{
    private List<CarInfo> _AvailableCars = new(); 
    public List<CarInfo> AvailableCars { get { return _AvailableCars;  } set { _AvailableCars = value; } }

    public IInventoryRepository _InventoryRepository;

    public event Action OnPurchaseSucceed;
    public InventoryManager()
    {
        _InventoryRepository = new InventoryRepository();
        _AvailableCars = _InventoryRepository.LoadAvailableCarData();
       
        PurchaseEvent.OnPurchase += AddBoughtCar;
    }

    private void AddBoughtCar(CarInfo BoughtCarDetails)
    {
       
        
        if(!_AvailableCars.Exists(c => c.CarID == BoughtCarDetails.CarID))
        {
            _AvailableCars.Add(BoughtCarDetails);
         
            if (_AvailableCars.Count >= 2)
            {
                _AvailableCars.Sort(SortByID);
            }
           
            _InventoryRepository.SaveAvailableCarData(_AvailableCars);

            OnPurchaseSucceed?.Invoke();
        }
        
    }

    int SortByID(CarInfo A , CarInfo B)
    {
        return B.CarID.CompareTo(A.CarID);
    }

}
