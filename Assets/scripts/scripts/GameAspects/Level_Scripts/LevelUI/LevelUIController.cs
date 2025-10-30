using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUIController : MonoBehaviour
{
    LevelUIModel model;
    LevelUIView view;

    [SerializeField] private Animator Transition;
    // Start is called before the first frame update
    void Start()
    {
        model = new LevelUIModel();
        view = FindObjectOfType<LevelUIView>();

        model.OnValueChange += ChangeViewStatus;

        view.OnBackToMainMenuClick += GoToMainMenu;
        view.OnBackToRaceSelectionClick += BackToSelectionEffect;
        view.OnDriftSelectionClick += DriftSelectionEffect;
        view.OnSimpleRaceSelectionClick += SimpleRaceSelectionEffect;
    }

    private void ChangeViewStatus(LevelUIModel model)
    {
        StartCoroutine(UpdateUI(model));
    }
    
    private void DriftSelectionEffect()
    {
        model.UpdateRaceSelectionActivation(false);
        model.UpdateLevelSelectionActivation(true);
        model.UpdateDriftLevelsActivation(true);
    }

    private void SimpleRaceSelectionEffect()
    {
        model.UpdateRaceSelectionActivation(false);
        model.UpdateLevelSelectionActivation(true);
        model.UpdateSimpleRaceLevelsActivation(true);
    }

    private void BackToSelectionEffect()
    {
        model.UpdateRaceSelectionActivation(true);
        model.UpdateLevelSelectionActivation(false);
        model.UpdateSimpleRaceLevelsActivation(false);
        model.UpdateDriftLevelsActivation(false);
    }


    private void GoToMainMenu()
    {
        //mainmenu 
    }

    IEnumerator UpdateUI(LevelUIModel model)
    {
        Transition.SetTrigger("start");

        yield return new WaitForSeconds(1f);

        view.RaceSelectionCanvasActivation(model.IsRaceSelectionCanvasEnabled);
        view.LevelSelectionCanvasActivation(model.IsLevelSelectionCanvasEnabled);
        view.DriftLevelsActivation(model.IsDriftLevelSelected);
        view.SimpleLevelsActivation(model.IsSimpleRaceLevelSelected);

        yield return new WaitForSeconds(1f);

        Transition.SetTrigger("end");

    }
}
