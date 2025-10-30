using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUIView : MonoBehaviour
{
    [SerializeField] private Button BackToMainMenu;
    [SerializeField] private Button BackToRaceSelection;
    [SerializeField] private Button DriftSelection;
    [SerializeField] private Button SimpleRaceSelection;

    public event Action OnBackToMainMenuClick;
    public event Action OnBackToRaceSelectionClick;
    public event Action OnDriftSelectionClick;
    public event Action OnSimpleRaceSelectionClick;


    [SerializeField] private Canvas RaceSelectionCanvas;
    [SerializeField] private Canvas LevelSelectionCanvas;

    [SerializeField] private GameObject DriftLevels;
    [SerializeField] private GameObject SimpleLevels;
    // Start is called before the first frame update
    void Start()
    {
        BackToMainMenu.onClick.AddListener(()=> OnBackToMainMenuClick?.Invoke());

        BackToRaceSelection.onClick.AddListener(() => OnBackToRaceSelectionClick?.Invoke());

        DriftSelection.onClick.AddListener(() => OnDriftSelectionClick?.Invoke());

        SimpleRaceSelection.onClick.AddListener(() => OnSimpleRaceSelectionClick?.Invoke());
    }

    public void RaceSelectionCanvasActivation(bool Enable)
    {
        RaceSelectionCanvas.enabled = Enable;
    }
    
    public void LevelSelectionCanvasActivation(bool Enable)
    {
        LevelSelectionCanvas.enabled = Enable;
    }

    public void DriftLevelsActivation(bool Enable)
    {
        DriftLevels.SetActive(Enable);
        ServiceLocator.Instance.GetService<RaceManager>().raceType = RaceType.DriftRace;
    }

    public void SimpleLevelsActivation(bool Enable)
    {
        SimpleLevels.SetActive(Enable);
        ServiceLocator.Instance.GetService<RaceManager>().raceType = RaceType.SimpleRace;
    }
}
