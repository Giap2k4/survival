using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SellectMapManager
{
    public static event Action<SellectMapModel> sellectMap;

    public static void HandleSellectMap(SellectMapModel model)
    {
        sellectMap?.Invoke(model);
    }
}
