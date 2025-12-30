using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthFundManager : DataPlayer<GrowthFundData>
{
    /// <summary>
    /// return TRUE: đã claim
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool CheckClaimed(int id)
    {
        return PlayerDataManager.GrowthFund.database.data.Contains(id);
    }

    public override void SetDataDefault()
    {
        base.SetDataDefault();
        PlayerDataManager.GrowthFund.database.data.Add(1);
        PlayerDataManager.GrowthFund.Save();
    }

    public static void Claimed(int id)
    {
        PlayerDataManager.GrowthFund.database.data.Add(id);
        PlayerDataManager.GrowthFund.Save();
    }
}
