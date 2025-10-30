using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class EliminationHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text EliminationTimer;
    [SerializeField] private TMP_Text TimeBeforeElimination_txt;
    [SerializeField] private TMP_Text NotifyElminationBegun;

    [SerializeField] private TMP_Text RankPosition;
    [SerializeField] private TMP_Text RemainingCars;

    [SerializeField] private TMP_Text NotifyElimination;



    StringBuilder builder;

    GameplayManager gameplayManager;

    // Start is called before the first frame update
    void Start()
    {
        gameplayManager = FindAnyObjectByType<GameplayManager>();
        RemainingCars.text = ServiceLocator.Instance.GetService<LevelManager>().selectedLevel.NumberOfCars.ToString();

        builder = new StringBuilder();

        gameplayManager.raceEvent.Register(NotifyEliminationAndRemainingCars);

        TimeBeforeElimination_txt.text = "60 s";

        StartCoroutine(TimeBeforeEliminationBegins());

    }

    // Update is called once per frame
    void Update()
    {
        UpdateEliminationTimer();
    }

    void UpdateEliminationTimer()
    {
        builder.Clear();
        builder.AppendFormat("{0:F2}",gameplayManager.raceEvent.EliminationTimer);


        EliminationTimer.text = builder.ToString() + " s";

        builder.Clear();

        builder.Append(gameplayManager.raceEvent.RankPositionOfPlayer);

        RankPosition.text = builder.ToString();

        builder.Clear();

        builder.Append("/");
        builder.Append(gameplayManager.raceEvent.RemainingCars);

        RemainingCars.text = builder.ToString();
        
    }
    void NotifyEliminationAndRemainingCars(string name , int remainingCars)
    {
        RemainingCars.text = "/ " + remainingCars.ToString() ;
        StartCoroutine(Notify(name));
                
    }

    IEnumerator Notify(string name)
    {
        NotifyElimination.text = $"{name} GOT ELIMINATED!!";
        NotifyElimination.gameObject.SetActive(true) ;

        if (this == null)
        {
            NotifyElimination.gameObject.SetActive(false);
            
            yield break;
        }
        yield return new WaitForSeconds(3f);

        NotifyElimination.gameObject.SetActive(false);
    }

  
       
    
    IEnumerator TimeBeforeEliminationBegins()
    {
        while (gameplayManager.raceEvent.TimeBeforeElimination > 0f)
        {
            builder.Clear();

            builder.Append($"{gameplayManager.raceEvent.TimeBeforeElimination:F1} s");

            TimeBeforeElimination_txt.text = builder.ToString();

            yield return null; 
        }

        
        TimeBeforeElimination_txt.gameObject.SetActive(false);
        TimeBeforeElimination_txt.gameObject.transform.parent.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.3f);
        NotifyElminationBegun.gameObject.SetActive(true);

        yield return new WaitForSeconds(3f);
        NotifyElminationBegun.gameObject.SetActive(false);
    }
}
