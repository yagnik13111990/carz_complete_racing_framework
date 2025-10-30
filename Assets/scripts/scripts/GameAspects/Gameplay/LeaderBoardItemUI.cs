using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class LeaderBoardItemUI : MonoBehaviour
{
    [SerializeField] TMP_Text Rank;
    [SerializeField] TMP_Text Name;
    [SerializeField] TMP_Text Time;

    public void SetItem(int rank, string name, float time)
    {
        Rank.text = rank.ToString();
        Name.text = name;

        int total = (int)time * 1000;

        int minutes = (total / 60000) % 60;
        int seconds = (total % 1000) % 60;
        int miliseconds = (total / 1000);

        Time.text = string.Format("{0:00 , 1:00 , 2:000}", minutes, seconds, miliseconds);
    }
}
