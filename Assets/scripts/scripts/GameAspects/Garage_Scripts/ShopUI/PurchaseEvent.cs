using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PurchaseEvent 
{
    public static event Action<CarInfo> OnPurchase;

    public static void RaisePurchaseEvent(CarInfo info)
    {
        OnPurchase?.Invoke(info);
    }
}
