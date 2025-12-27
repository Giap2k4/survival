using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : DataPlayer<ResourceData>
{

    /// <summary>
    /// Check có đủ tiền để mua k, nếu đủ hiển thị popup nhận phần thưởng vừa mua
    /// </summary>
    /// <param name="res"></param>
    public static void ResourceUse(PackageReward res)
    {

    }

    public override void SetDataDefault()
    {
        base.SetDataDefault();
        foreach (var item in DataManager.Resource.GetAll())
        {
            PlayerDataManager.Resource.database.resources.Add(item.package);
        }
        PlayerDataManager.Resource.Save();
    }
}
