using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : FeatureBaseController
{
    [Header("Cấu hình riêng")]
    [SerializeField]
    protected GameObject prefab;

    [SerializeField]
    protected RectTransform parentPrefab;

    protected override void LoadData()
    {
        base.LoadData();
        var allData = DataManager.Hero.GetAll();

        foreach (var item in allData)
        {
            GameObject obj = Instantiate(prefab, parentPrefab);
            var component = obj.GetComponent<HeroItemController>();
            component.InitData(item);
        }
    }
}
