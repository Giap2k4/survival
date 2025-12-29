using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemEquipmentController : MonoBehaviour
{
    [SerializeField]
    protected int id;

    [SerializeField]
    protected Button btn;

    protected void Start()
    {
        btn.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        var obj = UIManager.instance.OpenFeature(EnumBase.Feature.UpgradeEquip);
        obj.GetComponent<UpgradeEquipController>().SetData(id);
    }
}
