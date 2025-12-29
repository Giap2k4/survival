using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeEquipController : FeatureBaseController
{
    [SerializeField]
    protected TextMeshProUGUI txtName;

    [SerializeField]
    protected TextMeshProUGUI txtLevel;

    [SerializeField]
    protected Image image;

    [SerializeField]
    protected GameObject eff;

    [SerializeField]
    protected Transform parent;

    public void SetData(int idEquip)
    {
        var obj = DataManager.Equipment.GetById(idEquip);
        txtName.text = obj.name;
        image.sprite = Resources.Load<Sprite>("Equipment/" + obj.id);

        foreach (var item in obj.details)
        {
            GameObject prefab = Instantiate(eff, parent);
            prefab.GetComponent<ItemEffectController>().SetData(item, obj.id);
        }
    }
}
