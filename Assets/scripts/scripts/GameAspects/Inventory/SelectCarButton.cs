using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class SelectCarButton : MonoBehaviour
{
    public CarInfo carInfo;

    public void SelectedCar()
    {
        ServiceLocator.Instance.GetService<RaceManager>().selectedCar = carInfo;
        this.gameObject.GetComponent<Button>().interactable = false;
    }
}
