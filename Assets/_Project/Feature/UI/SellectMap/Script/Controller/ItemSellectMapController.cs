using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSellectMapController : MonoBehaviour
{
    [SerializeField]
    protected Image image;

    [SerializeField]
    protected TextMeshProUGUI txtNameMap;

    [SerializeField]
    protected Button btnClick;

    protected SellectMapModel modelObj;

    protected void Start()
    {
        btnClick.onClick.AddListener(OnClick);
    }

    public void SetData(SellectMapModel model)
    {
        modelObj = model;
        image.sprite = Resources.Load<Sprite>("Icon/icon_map/" + model.id);
        txtNameMap.text = model.nameMap;
        // check xem đã mở map đó chưa
    }

    public SellectMapModel GetModel() => modelObj;
    public void OnClick()
    {
        // chọn map và thoát feature sellect map
        SellectMapManager.HandleSellectMap(modelObj);
        UIManager.instance.CloseFeature(EnumBase.Feature.SellectMap);
    }
}
