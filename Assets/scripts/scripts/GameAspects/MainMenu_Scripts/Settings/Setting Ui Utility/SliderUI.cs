using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderUI : MonoBehaviour
{
    private Slider slider;
    [SerializeField] private TMP_Text num;
    private float value;
  
    public event Action<float> SliderChanged;
    public float Value => value;
    // Start is called before the first frame update
    void Awake()
    {
        
        slider = GetComponentInChildren<Slider>();
        slider.maxValue = 25f;

        slider.onValueChanged.AddListener(HandleValueChangeEvent);
    }

    void SliderTextUpdate(float val)
    {
        num.text = val.ToString("0.#");
    }
    void HandleValueChangeEvent(float val)
    {
        value = val;
        SliderTextUpdate(value);

        SliderChanged?.Invoke(value);
    }

    public void SetValue(float val)
    {
        if (slider == null) return;

        slider.onValueChanged.RemoveListener(HandleValueChangeEvent);

        value = val;
        slider.value = value;

        SliderTextUpdate(value);

        slider.onValueChanged.AddListener(HandleValueChangeEvent);
    }
}
