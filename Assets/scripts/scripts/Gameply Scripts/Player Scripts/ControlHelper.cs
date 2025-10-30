using UnityEngine;


public class ControlHelper : MonoBehaviour
{
    private MidRaceCalculations MidRaceCalculations;

    private ITrackUtility trackUtility;

    private ICommonCarEntity commonCarEntity;

    private int CurrentZoneIndex;
    private int SegmentCount;

    private float SegmentT;
    private float CurrentT;

   
   
    // Start is called before the first frame update
    void Start()
    {
       MidRaceCalculations = GetComponent<MidRaceCalculations>(); 

       trackUtility = FindAnyObjectByType<TrackUtility>();

       commonCarEntity = GetComponent<CommonEntity>();
    }

    // Update is called once per frame
    void Update()
    {
        GetCurrentIndex();

        IndexOfCurrentZone();

        ReactAccordingToTrackDetails();
    }

    int GetCurrentIndex()
    {
        CurrentT = MidRaceCalculations.NearestPoint;

        SegmentCount = trackUtility.TrackSpline.Count - 1;

        SegmentT = (float)1 / SegmentCount;

        int CurrentIndex = Mathf.FloorToInt(CurrentT / SegmentT);



        return CurrentIndex;
    }
    void IndexOfCurrentZone()
    {
        CurrentZoneIndex = GetCurrentIndex();

        int KnotIndex = CurrentZoneIndex;

        for (int i = 0; i < trackUtility.TrackZones.Count; i++)
        {
            if (KnotIndex >= trackUtility.TrackZones[i].start_index && KnotIndex <= trackUtility.TrackZones[i].end_index)
            {
                CurrentZoneIndex = i; break;
            }
        }
    }

    void ReactAccordingToTrackDetails()
    {
        TrackZone CurrentZone = trackUtility.TrackZones[CurrentZoneIndex];
        TrackZone NextZone = trackUtility.TrackZones[CurrentZoneIndex + 1];

        switch (CurrentZone.type)
        {
            case TrackType.Straight:

                commonCarEntity.SpeedLimit = commonCarEntity.MaximumSpeed;

                if (NextZone.type == TrackType.HairpinTurn || NextZone.type == TrackType.SharpTurn)
                {
                    commonCarEntity.SpeedLimit = commonCarEntity.MaximumSpeed * 0.60f;
                    commonCarEntity.TorqueLimit = commonCarEntity.MaximumTorque * 0.60f;
                 
                }

            break;

            case TrackType.GentleTurn:

                commonCarEntity.SpeedLimit = commonCarEntity.MaximumSpeed * 0.90f;
                commonCarEntity.TorqueLimit = commonCarEntity.MaximumTorque * 0.90f;

                break;

            case TrackType.MediumTurn:

                commonCarEntity.SpeedLimit = commonCarEntity.MaximumSpeed * 0.75f;
                commonCarEntity.TorqueLimit = commonCarEntity.MaximumTorque * 0.75f;

                if (NextZone.type == TrackType.HairpinTurn || NextZone.type == TrackType.SharpTurn)
                {
                    commonCarEntity.SpeedLimit = commonCarEntity.MaximumSpeed * 0.60f;
                    commonCarEntity.TorqueLimit = commonCarEntity.MaximumTorque * 0.60f;
                }

            break;


            case TrackType.SharpTurn:

                commonCarEntity.SpeedLimit = commonCarEntity.MaximumSpeed * 0.60f;
                commonCarEntity.TorqueLimit = commonCarEntity.MaximumTorque * 0.60f;

                break;

            default:

                commonCarEntity.SpeedLimit = commonCarEntity.MaximumSpeed * 0.35f;
                commonCarEntity.TorqueLimit = commonCarEntity.MaximumTorque * 0.35f;

                break;


        }

    }
}
