using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryRepository
{
    void SaveAvailableCarData (List<CarInfo> info);
    List<CarInfo> LoadAvailableCarData ();



}
