
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Profiling;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class SpawnManager : MonoBehaviour
{
    private AsyncOperationHandle<GameObject> operationHandle;
    private AsyncOperationHandle<GameObject> driftAIHandle;

    private ICarFactory Player_carFactory;
    private ICarFactory AI_carFactory;


    private CarRaceStats playerCarRaceData;
    private CarRaceStats AICarRaceData;

    private SpawnPositions spawnPositions;

    private Scene selectedLevelScene;

    private byte index;

    private LevelData level;

    private CarInfo selectedCar;

   
    public void Initialize()
    {
        level = ServiceLocator.Instance.GetService<LevelManager>().selectedLevel;

        selectedLevelScene = SceneManager.GetSceneByName(level.SceneName);

        foreach (GameObject obj in selectedLevelScene.GetRootGameObjects())
        {
            if (obj.TryGetComponent<SpawnPositions>(out var foundSpawnPositions))
            {
                spawnPositions = foundSpawnPositions;
                break;
            }
        }

        Player_carFactory = new PlayerCarFactory();
        AI_carFactory = new AICarFactory();

        selectedCar = ServiceLocator.Instance.GetService<RaceManager>().selectedCar;

        if (level.raceType == RaceType.SimpleRace)
        {
            operationHandle = Addressables.LoadAssetAsync<GameObject>(selectedCar.CarID.ToString());
            operationHandle.Completed += SpawnCars;
        }

        else
        {
            operationHandle = Addressables.LoadAssetAsync<GameObject>(selectedCar.CarID.ToString());
            operationHandle.Completed += SpawnDriftPlayerCar;

            driftAIHandle = Addressables.LoadAssetAsync<GameObject>("AI Drift " + selectedCar.CarID.ToString());
            driftAIHandle.Completed += SpawnDriftAICars;
        }

    }

    //race cars
    void SpawnCars(AsyncOperationHandle<GameObject> handle)
    {
        index = 0;

         if (handle.Status == AsyncOperationStatus.Succeeded)
         {
                for (index = 0; index < level.NumberOfCars - 1; index++)
                {
                    GameObject AICar = AI_carFactory.CreateCar(handle.Result, spawnPositions.Positions[index],
                        level.SpawnAngle);

                    AICar.name = $"Opponenent {index + 1}";

                    ICommonCarEntity commonCarEntity = AICar.GetComponent<CommonEntity>();

                    commonCarEntity.InitializeDamper(Random.Range(0.85f , 0.95f));


                AIPathTraker tracker = AICar.GetComponent<AIPathTraker>();
                   tracker.BaseLookAheadPointDistance = 30f;

                if (index % 2 == 0) tracker.LaneOffsetRange = (float)UnityEngine.Random.Range(-2, 0);
                else tracker.LaneOffsetRange = (float)UnityEngine.Random.Range(0, 2);

                   AICar.transform.Find("arrow").gameObject.SetActive(true);

                }

                GameObject PlayerCar = Player_carFactory.CreateCar(handle.Result, spawnPositions.Positions[index], level.SpawnAngle);

                PlayerCar.name = "You";
                AssignTag(PlayerCar, "Player");

                ICommonCarEntity commonPlayerCarEntity = PlayerCar.GetComponent<CommonEntity>();

                commonPlayerCarEntity.InitializeDamper(1f);

                PlayerCar.transform.Find("arrow").gameObject.SetActive(true);

        }

    }


    //drift cars
    void SpawnDriftAICars(AsyncOperationHandle<GameObject> handle)
    {
        index = 0;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            for (index = 0; index < level.NumberOfCars - 1; index++)
            {
                GameObject AICar = Instantiate(handle.Result, spawnPositions.Positions[index], Quaternion.Euler(0f, level.SpawnAngle, 0f));

                AICar.name = $"Opponenent {index + 1}";

                AICar.AddComponent<AIPathTraker>();
                AICar.GetComponent<AIPathTraker>().enabled = false;

                AICar.AddComponent<AIDriftHelper>();
                AICar.GetComponent<AIDriftHelper>().enabled = false;

                AICar.GetComponent<AIPathTraker>().BaseLookAheadPointDistance = 5f;

                AICar.GetComponent<Drift>().InitializeDamper(Random.Range(0.85f, 0.95f));

              //  AICar.transform.Find("arrow").gameObject.SetActive(true);
            }
        }
    }

    void SpawnDriftPlayerCar(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject PlayerCar = Player_carFactory.CreateCar(handle.Result, spawnPositions.Positions[level.NumberOfCars - 1], level.SpawnAngle);

            PlayerCar.name = "You";
            AssignTag(PlayerCar, "Player");

            ICommonCarEntity commonPlayerCarEntity = PlayerCar.GetComponent<CommonEntity>();

            commonPlayerCarEntity.InitializeDamper(1f);

            PlayerCar.transform.Find("arrow").gameObject.SetActive(true);
        }
    }
    private void AssignTag(GameObject obj , string tag)
    {
        obj.tag = tag;
    }
    private void OnDestroy()
    {

        if (level.raceType == RaceType.SimpleRace) operationHandle.Completed -= SpawnCars;

        else { operationHandle.Completed -= SpawnDriftAICars; operationHandle.Completed -= SpawnDriftPlayerCar; }

        if (operationHandle.IsValid())
        {
            Addressables.Release(operationHandle);
        }
    }
}
