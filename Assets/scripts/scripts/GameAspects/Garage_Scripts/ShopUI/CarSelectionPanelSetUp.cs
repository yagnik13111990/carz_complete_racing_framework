using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSelectionPanelSetUp : MonoBehaviour
{
    [SerializeField] private List<Sprite> carImages = new List<Sprite>();

    [SerializeField] private GameObject ButtonPrefab;

    [SerializeField] private RectTransform Content;

    private IObservableCarSelectionButton carSelectionButton;
    void Awake()
    {
        CollectionOfObservables.Observables.Clear();
        foreach (Sprite sprite in carImages)
       {
            GameObject Obj = Instantiate(ButtonPrefab , Content);
            carSelectionButton = Obj.GetComponent<ObservableCarSelectionButton>();
            carSelectionButton.SetButtonImage(sprite);
            CollectionOfObservables.Observables.Add(carSelectionButton);
       }
    }

   
}
