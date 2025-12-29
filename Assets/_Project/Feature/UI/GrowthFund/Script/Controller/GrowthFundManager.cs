using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthFundManager : DataPlayer<GrowthFundData>
{
    /// <summary>
    /// Set level mới vượt qua
    /// </summary>
    /// <param name="id"></param>
    public static void SetMapUnlock(int id)
    {
        PlayerDataManager.GrowthFund.database.data.Add(id, false);
        PlayerDataManager.GrowthFund.Save();
    }

    public static void ClaimReward(PackageReward res)
    {

    }

    /// <summary>
    /// return TRUE: đã claim
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool CheckClaimed(int id)
    {
        return PlayerDataManager.GrowthFund.database.data[id];
    }

    public override void SetDataDefault()
    {
        base.SetDataDefault();
        PlayerDataManager.GrowthFund.database.data.Add(1, true);
        PlayerDataManager.GrowthFund.Save();
    }
}
