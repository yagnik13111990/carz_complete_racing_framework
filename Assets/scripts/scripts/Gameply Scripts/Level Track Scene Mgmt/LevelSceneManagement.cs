
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSceneManagement : MonoBehaviour
{

    string SceneName;

    ITrackUtility trackUtility;

    SpawnManager spawnManager;

    GameplayManager gameplayManager;

    HUDController hudController;

    FollowCar carFollower;

    private void Awake()
    {
        SceneName = ServiceLocator.Instance.GetService<LevelManager>().selectedLevel.SceneName;

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(SceneName , LoadSceneMode.Additive);
    }

    void OnSceneLoaded(Scene scene , LoadSceneMode mode)
    {
        if (scene.name != SceneName) return;


        SceneManager.sceneLoaded -= OnSceneLoaded;

        trackUtility = FindFirstObjectByType<TrackUtility>();
        trackUtility.Initialize();

        spawnManager = FindFirstObjectByType<SpawnManager>();
        spawnManager.Initialize();

        gameplayManager = FindFirstObjectByType<GameplayManager>();
        gameplayManager.Initialize();

        hudController = FindFirstObjectByType<HUDController>();
        hudController.Initialize();

        carFollower = FindAnyObjectByType<FollowCar>();
        carFollower.Initialize();

       
    }
}
