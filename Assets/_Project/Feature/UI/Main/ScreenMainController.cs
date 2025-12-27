using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ScreenMainController : MonoBehaviour
{
    [SerializeField]
    protected TextMeshProUGUI txtEnery;

    [SerializeField]
    protected TextMeshProUGUI txtDiamond;

    [SerializeField]
    protected TextMeshProUGUI txtGold;

    // xử lý noti ở các btn feature khác
    protected void Start()
    {
        var enery = PlayerDataManager.Resource.database.resources.FirstOrDefault(x => x.resType == EnumBase.ResourcesType.Money && x.resId == 3).resQuantity;
        txtEnery.text = enery.ToString() + "/150";

        var diamond = PlayerDataManager.Resource.database.resources.FirstOrDefault(x => x.resType == EnumBase.ResourcesType.Money && x.resId == 2).resQuantity;
        txtDiamond.text = diamond.ToString();

        var gold = PlayerDataManager.Resource.database.resources.FirstOrDefault(x => x.resType == EnumBase.ResourcesType.Money && x.resId == 1).resQuantity;
        txtGold.text = gold.ToString();
    }

}
