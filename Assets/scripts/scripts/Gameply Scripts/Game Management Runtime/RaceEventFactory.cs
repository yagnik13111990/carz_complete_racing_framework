using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceEventFactory : IRaceRuleFactory
{
    public RaceEvents CreateRaceRule(string raceRuleName)
    {
       switch (raceRuleName)
        {
            case "Sprint":
                return new SprintRace();

            case "Elimination":
                return new EliminationRace();

            case "OneVOne":
                return new OneVOne();


            case "LapRace":
                return new LapRace();


            default:
                return new TimeTrial();


        }
    }

   
}
