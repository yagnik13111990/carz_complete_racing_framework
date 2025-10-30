//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;


//public class ToggleUI : MonoBehaviour
//{
//    private Toggle Toggle;
//    private bool _isOn;  

//    public bool IsOn => _isOn;

//    public event Action<bool> OnToggleChanged;



//    private void Awake()
//    {
//        Toggle = GetComponentInChildren<Toggle>();
//        Toggle.onValueChanged.AddListener(OnValueChangeEventHandler);
//    }

//    void OnValueChangeEventHandler(bool _isOn_)
//    {
//       _isOn = _isOn_;
//        OnToggleChanged?.Invoke(_isOn);
//    }

//    public void SetValue(bool value)
//    {
//        Toggle.onValueChanged.RemoveListener(OnValueChangeEventHandler);
//        _isOn = value;
//        Toggle.isOn = _isOn;
//        Toggle.onValueChanged.AddListener(OnValueChangeEventHandler);
//    }
//}

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ToggleUI : MonoBehaviour
{
    [SerializeField] private Image Bar;

    private float ThreshHoldTime;
    private float t;
    private float StartPos = -26f;
    private float EndPos = 26f;
    private Vector3 position;

    private Toggle toggle;
    private bool _isOn;

    private Coroutine coroutine;
    public bool IsOn => _isOn;

    public event Action<bool> OnToggleChanged;

    private void Awake()
    {
        toggle = GetComponentInChildren<Toggle>();

       
        if (toggle != null)
            toggle.onValueChanged.AddListener(OnValueChangeEventHandler);
    }

    private void OnDestroy()
    {
        if (toggle != null)
            toggle.onValueChanged.RemoveListener(OnValueChangeEventHandler);
    }

    private void OnValueChangeEventHandler(bool newValue)
    {
        if (_isOn == newValue) return;
       
        _isOn = newValue;
        AnimateBarToValue(_isOn);

        OnToggleChanged?.Invoke(_isOn);
    }

    private void AnimateBarToValue(bool on)
    {
        if (coroutine != null) StopCoroutine(coroutine);

        StartPos = on ? -26f : 26f;
        EndPos = on ? 26f : -26f;

        coroutine = StartCoroutine(AnimateBar(StartPos, EndPos));
    }

    IEnumerator AnimateBar(float start , float end)
    {
        t = 0;

        position = Bar.transform.localPosition;

       while(t < ThreshHoldTime)
       {
            t += Time.deltaTime;

            position.x = Mathf.Lerp(start, end, t / ThreshHoldTime);
            Bar.transform.localPosition = position;

            yield return null;
       }

        position.x = end;
        Bar.transform.localPosition = position;
    }

    public void SetValue(bool value)
    {
        if (toggle == null) return;

        toggle.onValueChanged.RemoveListener(OnValueChangeEventHandler);

        _isOn = value;     
        toggle.isOn = _isOn;

        AnimateBarToValue(_isOn);

        toggle.onValueChanged.AddListener(OnValueChangeEventHandler);
    }
}
