using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ControlSettingView : MonoBehaviour
{

    private List<CycleSelector<KeyCode>> _InputCycleSelectors;

    public List<CycleSelector<KeyCode>> InputCycleSelectors { get => _InputCycleSelectors; set => _InputCycleSelectors = value; }

    private List<List<KeyCode>> InputControls = new List<List<KeyCode>>
    {
         // Acceleration
         new List<KeyCode> { KeyCode.W, KeyCode.UpArrow, KeyCode.N },
         
         // Brake
         new List<KeyCode> { KeyCode.S, KeyCode.DownArrow, KeyCode.M },
         
         // Steer Left
         new List<KeyCode> { KeyCode.A, KeyCode.LeftArrow, KeyCode.LeftBracket },
         
         // Steer Right
         new List<KeyCode> { KeyCode.D, KeyCode.RightArrow, KeyCode.RightBracket },
         
         // Switch Camera
         new List<KeyCode> { KeyCode.C, KeyCode.V, KeyCode.Insert },
         
         // Short Brake / Handbrake
         new List<KeyCode> { KeyCode.Space, KeyCode.K, KeyCode.RightShift },

    };

    private void Awake()
    {
        InputCycleSelectors = GetComponentsInChildren<CycleSelector<KeyCode>>().ToList();

       for(int i = 0; i < InputControls.Count; i++)
        {
            int index = i;
            InputCycleSelectors[index].ContentSetUp(new ListCycleSelection<KeyCode>(InputControls[index]));


        }
    }
}
