using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class RaceHUD : MonoBehaviour
{
    [SerializeField] private Button Restart;
    [SerializeField] private Button Resume;
    [SerializeField] private Button MainMenu;
    [SerializeField] private Button Pause;
    [SerializeField] private TMP_Text FinishNote;

    [SerializeField] private GameObject PauseCanvas;

    public TMP_Text CountDown;

   
    public event Action OnResumeClick;
    public event Action OnPauseClick;



    // Start is called before the first frame update
    void Awake()
    {
       
        Resume.onClick.AddListener(ResumeAction);
        Pause.onClick.AddListener(PausePanelAction);
    }

    public void EnableMessageOfFinish(bool enable)
    {
        FinishNote.gameObject.SetActive(enable);
    }
  
    void ResumeAction()
    {
        OnResumeClick?.Invoke();
    }

  
    void PausePanelAction()
    {
        OnPauseClick?.Invoke();
    }

    public void ShowPausePanel(bool show)
    {
        PauseCanvas?.SetActive(show);

    }

    private void OnDisable()
    {
        OnResumeClick = null;
        OnPauseClick = null;
    }
}
