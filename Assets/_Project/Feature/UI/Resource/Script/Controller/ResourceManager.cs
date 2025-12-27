using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public static void ResourceUse(PackageReward[] res)
    {

    }

    public static void ResourcesCollect(List<PackageReward> res) 
    {
        var data = PlayerDataManager.Resource.database.resources;
        foreach (var item in res)
        {
            var obj = data.FirstOrDefault(x => x.resType == item.resType && x.resId == item.resId);
            if (obj == null)
            {
                data.Add(obj);
                continue;
            }

            obj.resQuantity += item.resQuantity;
        }
        
        PlayerDataManager.Resource.Save();
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
