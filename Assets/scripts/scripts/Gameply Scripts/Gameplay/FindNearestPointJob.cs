using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

[BurstCompile]
public struct FindNearestPointJob : IJob
{
    [ReadOnly] public NativeArray<float3> TrackPathPoints;
    [ReadOnly] public float3 CarPosition;
    public NativeArray<float> ResultT; // Use NativeArray for result

    public void Execute()
    {
        int NearestIndex = 0;
        float MinDist = float.MaxValue;

        for (int i = 0; i < TrackPathPoints.Length; i++)
        {
            float Dist = math.distancesq(CarPosition, TrackPathPoints[i]);
            if (Dist < MinDist)
            {
                NearestIndex = i;
                MinDist = Dist;
            }
        }

        ResultT[0] = (float)NearestIndex / (TrackPathPoints.Length - 1);
    }
}
