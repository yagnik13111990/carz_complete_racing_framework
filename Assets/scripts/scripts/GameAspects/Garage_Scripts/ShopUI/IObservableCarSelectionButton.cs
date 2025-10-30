using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObservableCarSelectionButton 
{
    CarScriptableObjData CarSO { get; set; }

    void LetSubscribe(IObserverOfSelectedCar ObserverOfSelection);
    void RemoveSubscriptionOf(IObserverOfSelectedCar ObserverOfSelection);

    void Notify();

    void SetCarDetail(CarScriptableObjData data);

    void SetButtonImage(Sprite sprite);

}
