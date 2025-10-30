using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager 
{
    private Dictionary<int, LevelData> _DriftLevels;
    private Dictionary<int, LevelData> _RaceLevels;

    public Dictionary<int, LevelData> DriftLevels { get => _DriftLevels; set => _DriftLevels = value; }
    public Dictionary<int, LevelData> RaceLevels { get => _RaceLevels; set => _RaceLevels = value; }

    private LevelData _data;

    public LevelData selectedLevel {
        get => _data; 
        set => _data = value;
        
    }

    ILevelRepository _driftLevelRepository;
    ILevelRepository _raceLevelRepository;

    public ILevelRepository DriftLevelRepository { get => _driftLevelRepository; set => _driftLevelRepository = value; }

    public ILevelRepository SimpleLevelRepository { get => _raceLevelRepository; set => _raceLevelRepository = value; }



    public LevelManager()
    {

        _driftLevelRepository = new DriftLevelRepository();
        _raceLevelRepository = new RaceLevelRepository();

        _DriftLevels = _driftLevelRepository.LoadLevelData();
        _RaceLevels = _raceLevelRepository.LoadLevelData();

        LevelSelectionEvent.OnLevelSelected += ChosenLevel;

    }
    public void ChosenLevel(LevelData data)
    {
        _data = data;
      
    }

    
}
