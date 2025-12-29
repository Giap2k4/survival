using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrowthFundController : FeatureBaseController
{
    [SerializeField]
    protected GameObject item;

    [SerializeField]
    protected Transform parent;

    protected override void LoadData()
    {
        base.LoadData();
        var data = DataManager.GrowthFund.GetAll();

        foreach (var obj in data.Reverse())
        {
            GameObject prefab = Instantiate(item, parent);
            prefab.GetComponent<ItemGrowthFundController>().SetData(obj);
        }
    }
}
