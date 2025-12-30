using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceManager : DataPlayer<ResourceData>
{
    public static event Action<EnumBase.ResourcesType, int> eventChangeRes;

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
            if (obj == null) data.Add(obj);

            obj.resQuantity += item.resQuantity;
            eventChangeRes?.Invoke(obj.resType ,obj.resId);
        }
        
        PlayerDataManager.Resource.Save();

        var feature = UIManager.instance.OpenFeature(EnumBase.Feature.Resource);
        feature.GetComponent<ResourceController>().SetData(res);
    }

    public static void ResourcesCollect(PackageReward res)
    {
        var data = PlayerDataManager.Resource.database.resources;
        var obj = data.FirstOrDefault(x => x.resType == res.resType && x.resId == res.resId);
        if (obj == null) data.Add(obj);
        obj.resQuantity += res.resQuantity;
        eventChangeRes?.Invoke(obj.resType ,obj.resId);

        PlayerDataManager.Resource.Save();

        var feature = UIManager.instance.OpenFeature(EnumBase.Feature.Resource);
        feature.GetComponent<ResourceController>().SetData(res);
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

    public static int GetQuantityRes(EnumBase.ResourcesType type, int id)
    {
        return PlayerDataManager.Resource.database.resources.First(x => x.resType == type && x.resId == id).resQuantity;
    }
}
