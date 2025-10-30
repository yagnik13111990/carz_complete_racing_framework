using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelManager : MonoBehaviour
{
    [SerializeField] private GameObject CurrentPanel;
   

    public void OpenPanel(GameObject panel)
    {
        if(CurrentPanel != null) CurrentPanel.SetActive(false);

        panel.SetActive(true);
        CurrentPanel = panel;
    }

    public void Open(GameObject CurrentPanel)
    {
        CurrentPanel.SetActive(true);
    }

    public void Close(GameObject CurrentPanel)
    {
        CurrentPanel.SetActive(false);
    }
}
