using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DropdownUI : MonoBehaviour 
{
    private Button Option;

    private List<KeyCode> DropDownOptions = new List<KeyCode>();

    private TMP_Dropdown dropdown;

    private KeyCode _keyCode;

    public KeyCode SelectedKeyCode { get => _keyCode; set => _keyCode = value; }

    public event Action<KeyCode> DropdownChanged;
   
    void Start()
    {
        Option = GetComponent<Button>();

        Option.onClick.AddListener(AssignDropdownOptions);

        dropdown.onValueChanged.AddListener(AssignSelectionOption);
    }
    public void SetOptions(List<KeyCode> options , TMP_Dropdown dp)
    {
        DropDownOptions = options;
        dropdown = dp;

    }

    public void SetValue(KeyCode value)
    {
        int index = DropDownOptions.IndexOf(value);

        if(index >= 0)
        {
            dropdown.value = index;
            dropdown.RefreshShownValue();
            
            _keyCode = value;
        }
    }

    void AssignDropdownOptions()
    {
        dropdown.ClearOptions();
        //dropdown.AddOptions(DropDownOptions);
    }

    void AssignSelectionOption(int index)
    {
       if(Enum.TryParse<KeyCode>(dropdown.options[index].ToString(), out KeyCode key))
        {
            _keyCode = key;
            DropdownChanged?.Invoke(_keyCode);
        }
    }
}
