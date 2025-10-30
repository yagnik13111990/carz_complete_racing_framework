using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRaceRuleFactory 
{
    RaceEvents CreateRaceRule(string raceRuleName);
}
