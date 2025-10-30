using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class Minimap : MonoBehaviour
{
    ITrackUtility trackUtility;

    LineRenderer lineRenderer;

    GameObject PlayerCar;

    [SerializeField] private Transform _camera;

    Transform FollowAnchor;


    int Samples = 300;

    float height = 250f;

    float distance = 10f;

    float smooth = 5f;


    void Start()
    {
        if (!(bool)ServiceLocator.Instance.GetService<SettingManager>().M_Gameplay.GameplaySettings[GameplaySettingKey.Map])
        {
            _camera.gameObject.SetActive(false);
            this.gameObject.SetActive(true);

        }
        lineRenderer = GetComponent<LineRenderer>();

        trackUtility = FindAnyObjectByType<TrackUtility>();

        StartCoroutine(ConstructTrack());

        StartCoroutine(CreateGPSOfCars());
    }

    IEnumerator ConstructTrack()
    {
        yield return new WaitUntil(() => trackUtility.TrackCurve != null);

        lineRenderer.positionCount = Samples;

        lineRenderer.useWorldSpace = true;

        for(int i = 0; i < Samples; i++)
        {
            float t = (float)i / Samples;

            Vector3 LocalPosition = trackUtility.TrackCurve.EvaluatePosition(t);

            lineRenderer.SetPosition(i, LocalPosition);
        }

        lineRenderer.loop = true;
    }

    IEnumerator CreateGPSOfCars()
    {
      
            yield return new WaitUntil(() =>
            {
                var car = GameObject.FindGameObjectWithTag("Player");
                return car != null && car.transform.GetChild(0) != null;
            });

            PlayerCar = GameObject.FindGameObjectWithTag("Player");

            FollowAnchor = PlayerCar.transform.GetChild(0);

    }
    // Update is called once per frame
    void LateUpdate()
    {
        FollowPlayerGPS();
    }

    void FollowPlayerGPS()
    {
        if (FollowAnchor == null) return;

  
            Vector3 offset = - FollowAnchor.forward * distance + Vector3.up * height;
            Vector3 targetPos = FollowAnchor.position + offset;

           
            _camera.position = Vector3.Lerp(_camera.position, targetPos, Time.deltaTime * smooth);

            
            Quaternion topDownRotation = Quaternion.Euler(90f, FollowAnchor.eulerAngles.y, 0f);
            _camera.rotation = Quaternion.Lerp(_camera.rotation, topDownRotation, Time.deltaTime * smooth);
        



    }
}
