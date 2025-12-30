using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : FeatureBaseController
{
    [SerializeField]
    protected GameObject item;

    [SerializeField]
    protected Transform parent;


    public void SetData(List<PackageReward> listRes)
    {
        foreach (var data in listRes)
        {
            GameObject obj = Instantiate(item, parent);
            obj.GetComponent<ItemResourcesController>().SetData(data);
        }
    }

    public void SetData(PackageReward res)
    {
        GameObject obj = Instantiate(item, parent);
        obj.GetComponent<ItemResourcesController>().SetData(res);
    }
}
