//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;

//public class ObserverOfSelectedCar : MonoBehaviour ,  IObserverOfSelectedCar
//{

//   [SerializeField] private TMP_Text Price;
//   [SerializeField] private TMP_Text Name;

//   [SerializeField] private TMP_Text Acceleration;
//   [SerializeField] private TMP_Text MaxSpeed;
//   [SerializeField] private TMP_Text Torque;
//   [SerializeField] private TMP_Text Cornering;
//    public void UpdateStates (CarScriptableObjData _carSO)
//    {
//        Price.text = _carSO.Price.ToString() + " CR";
//        Name.text = _carSO.CarName;

//        Acceleration.text = _carSO.Acceleration.ToString();
//        MaxSpeed.text = _carSO.Maxspeed.ToString();
//        Torque.text = _carSO.Torque.ToString();
//        Cornering.text = _carSO.Cornering.ToString();



//    }

//}

using TMPro;
using UnityEngine;

public class ObserverOfSelectedCar : MonoBehaviour, IObserverOfSelectedCar
{
    [SerializeField] private TMP_Text Price;
    [SerializeField] private TMP_Text Name;
    [SerializeField] private TMP_Text Acceleration;
    [SerializeField] private TMP_Text MaxSpeed;
    [SerializeField] private TMP_Text Torque;
    [SerializeField] private TMP_Text Cornering;

    public void UpdateStates(CarScriptableObjData carSO)
    {
        if (carSO == null)
        {
            Debug.LogWarning("Received null CarSO!");
            return;
        }

        Price.text = carSO.Price.ToString() + " CR";
        Name.text = "Car" + carSO.CarID;
        Acceleration.text = carSO.Acceleration.ToString();
        MaxSpeed.text = carSO.Maxspeed.ToString();
        Torque.text = carSO.Torque.ToString();
        Cornering.text = carSO.Cornering.ToString();
    }
}
