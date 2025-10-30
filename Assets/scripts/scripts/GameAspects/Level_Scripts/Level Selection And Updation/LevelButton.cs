using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private Image[] stars = new Image[3];
    [SerializeField] private TMP_Text rank;
    [SerializeField] private TMP_Text id;
    [SerializeField] private TMP_Text raceEvent;

    private LevelData LevelData;
    public void SetLevelData (LevelData levelData)
    {
        LevelData = levelData;
        SetRankText();
        SetIDText();
        SetRatingText();
        SetRaceEventText();
        LockUnlock();

       
    }

    private void OnEnable()
    {
        this.GetComponent<Button>().onClick.AddListener(() => LevelSelectionEvent.NotifyLevelManager(LevelData));
    }

    private void OnDisable()
    {
        this.GetComponent<Button>().onClick.RemoveAllListeners();
    }

    private void LockUnlock()
    {
        if (LevelData.State == State.Locked) { this.GetComponent<Button>().interactable = false; }
        else this.GetComponent<Button>().interactable = true;
        
        // if(LevelData.State == State.IsCompleted) 
    }
    private void SetRankText()
    {
        if(LevelData.Rank > 0) rank.text = $"{LevelData.Rank}" + LevelData.Rank switch { 1 => "st", 2 => "nd", 3 => "rd", _ => "th" } ;
    }

    private void SetRatingText()
    {
        int starToLit = LevelData.Rank switch
        {
            1 => 3,
            2 => 2,
            3 => 1,
            _ => 0
        };

        for(int i = 0; i < starToLit; i++)
        {
            stars[i].color = new Color(1, 1, 1, 1);
        }
    }

    private void SetIDText()
    {
        id.text = $"{LevelData.LevelID:D2}";
    }

    private void SetRaceEventText()
    {
        raceEvent.text = $"{LevelData.RaceEvent}";
    }

   
}
