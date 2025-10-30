using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameplaySettingView : MonoBehaviour
{

    public SliderUI CamFollowSlider;

    public List<ToggleUI> toggleUIs = new();
    private void Awake()
    {
        toggleUIs = GetComponentsInChildren<ToggleUI>().ToList();
        CamFollowSlider = GetComponentInChildren<SliderUI>();


    }


}