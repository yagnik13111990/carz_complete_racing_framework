

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ObservableCarSelectionButton : MonoBehaviour, IObservableCarSelectionButton
{
    private List<IObserverOfSelectedCar> _observers = new List<IObserverOfSelectedCar>();

    private Button _button;

    private Image _image;
    public CarScriptableObjData CarSO { get; set; }

    

    void Awake()
    {

        _button = GetComponent<Button>();
        _image = GetComponent<Image>();

        _button.onClick.AddListener(Notify);
        _button.interactable = false; // Disable until asset assigned
       
    }
    public void SetButtonImage (Sprite sprite)
    {
        _image.sprite = sprite;
    }

    public void LetSubscribe(IObserverOfSelectedCar observer)
    {
        if (observer != null && !_observers.Contains(observer))
            _observers.Add(observer);
    }

    public void RemoveSubscriptionOf(IObserverOfSelectedCar observer)
    {
        if (_observers.Contains(observer))
            _observers.Remove(observer);
    }

    public void SetCarDetail(CarScriptableObjData carSO)
    {
        CarSO = carSO;
        _button.interactable = carSO != null; // Enable button
    }

    public void Notify()
    {
        if (_observers.Count == 0)
        {
            
            return;
        }

        if (CarSO == null)
        {
            
            return;
        }

        foreach (var observer in _observers)
        {
            observer.UpdateStates(CarSO);
        }

        
    }
}

