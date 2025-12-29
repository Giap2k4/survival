using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SellectMapController : FeatureBaseController
{
    [SerializeField]
    protected GameObject itemMap;

    [SerializeField]
    protected Transform parentItem;

    protected override void LoadData()
    {
        base.LoadData();
        var data = DataManager.SellectMap.GetAll();
        var dataRev = data.Reverse();

        foreach (var item in dataRev)
        {
            GameObject obj = Instantiate(itemMap, parentItem);
            obj.GetComponent<ItemSellectMapController>().SetData(item);
        }

        // xử lý click vào cuộn đúng map đang chọn
    }
}
