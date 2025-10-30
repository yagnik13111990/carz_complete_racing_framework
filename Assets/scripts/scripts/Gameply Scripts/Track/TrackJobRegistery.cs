using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Jobs;
using UnityEngine;

public static class TrackJobRegistery 
{
   public static List<JobHandle> ActiveHandles = new List<JobHandle>();

   public static void AddToRgisterOfHandle(JobHandle handle)
    {
        
        ActiveHandles.Add(handle);
    }


    public static void CompleteAllAtOnce()
    {
        foreach(JobHandle h in  ActiveHandles)
        {
            h.Complete();
        }
    }
}
