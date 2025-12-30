using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SevenDayLoginController : FeatureBaseController
{
    [Header("Cấu hình riêng")]
    [SerializeField]
    protected GameObject item;

    [SerializeField]
    protected Transform parent;

    protected override void LoadData()
    {
        base.LoadData();
        var data = DataManager.SevenDayLogin.GetAll();

        foreach (var obj in data)
        {
            GameObject prefab = Instantiate(item, parent);
            prefab.GetComponent<ItemSevenDayLoginController>().SetData(obj);
        }
    }
}
