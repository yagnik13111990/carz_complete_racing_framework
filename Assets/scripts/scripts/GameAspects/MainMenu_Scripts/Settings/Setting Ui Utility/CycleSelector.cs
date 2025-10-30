using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;



public class CycleSelector <T> : MonoBehaviour 
{
    [SerializeField] private Button LeftButton;
    [SerializeField] private Button RightButton;
    [SerializeField] private TMP_Text contentTxt;

    private ICycleSelector<T> selector;

    private T _CurrentSelected;

    private int index = 0;
    public T CurrentSelected => _CurrentSelected;

    public event Action<T> OnValueSelection;
    public void ContentSetUp(ICycleSelector<T> selector)
    {
        this.selector = selector;

        UpdateButtonState();

        if (LeftButton != null) LeftButton.onClick.AddListener(MovePrev);
        if (RightButton != null) RightButton.onClick.AddListener(MoveNext);
    }

    void MoveNext()
    {     
        if(selector == null || selector.Count == 0) return;
        index = Mathf.Clamp(index + 1, 0, selector.Count - 1);
 
        SetValue(selector.Get(index));
    }

    void MovePrev()
    {
        if (selector == null || selector.Count == 0) return;
      
        index = Mathf.Clamp(index - 1, 0, selector.Count - 1);
       
        SetValue(selector.Get(index));

    }

    public void SetValue(T value)
    {

        index = selector.IndexOf(value);

        _CurrentSelected = value;

        contentTxt.text = _CurrentSelected.ToString();

        UpdateButtonState();

        OnValueSelection?.Invoke(_CurrentSelected);
    }

    private void UpdateButtonState()
    {
        if (LeftButton != null) LeftButton.interactable = index > 0;
        if (RightButton != null) RightButton.interactable = index < selector.Count - 1;

    }

}
