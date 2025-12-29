using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenMainController : FeatureBaseController
{
    #region Cấu hình
    [SerializeField]
    protected TextMeshProUGUI txtEnery;

    [SerializeField]
    protected TextMeshProUGUI txtDiamond;

    [SerializeField]
    protected TextMeshProUGUI txtGold;
    #endregion

    // xử lý noti ở các btn feature khác
    protected override void Start()
    {
        SetDataItem();
    }

    protected void SetDataItem()
    {
        var enery = PlayerDataManager.Resource.database.resources.FirstOrDefault(x => x.resType == EnumBase.ResourcesType.Money && x.resId == 3).resQuantity;
        txtEnery.text = enery.ToString() + "/150";

        var diamond = PlayerDataManager.Resource.database.resources.FirstOrDefault(x => x.resType == EnumBase.ResourcesType.Money && x.resId == 2).resQuantity;
        txtDiamond.text = diamond.ToString();

        var gold = PlayerDataManager.Resource.database.resources.FirstOrDefault(x => x.resType == EnumBase.ResourcesType.Money && x.resId == 1).resQuantity;
        txtGold.text = gold.ToString();
    }

}
