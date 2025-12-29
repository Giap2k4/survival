using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellectLevelController : MonoBehaviour
{
    [SerializeField]
    protected TextMeshProUGUI txtNameMap;

    [SerializeField]
    protected Image imageSellectMap;

    protected void OnEnable()
    {
        SellectMapManager.sellectMap += HandleSellectMap;
    }

    protected void OnDisable()
    {
        SellectMapManager.sellectMap -= HandleSellectMap;
    }

    protected void Start()
    {
        var key = "SellectLevelManager"; // ví dụ
        var json = PlayerPrefs.GetString(key, "(missing)");
        Debug.Log(json);

        var levelCurrent = SellectLevelManager.GetLevelCurrent();
        var idMap = SellectLevelManager.GetIdMap();

        txtNameMap.text = levelCurrent.ToString() + ". " + SellectLevelManager.GetNameMap(idMap);
        imageSellectMap.sprite = Resources.Load<Sprite>("Icon/icon_map/" + idMap);
    }

    protected void HandleSellectMap(SellectMapModel model)
    {
        // xử lý đổi map
        var idMap = model.id;
        var data = DataManager.SellectMap.GetById(idMap);

        txtNameMap.text = data.levelStart.ToString() + ". " + data.nameMap;
        imageSellectMap.sprite = Resources.Load<Sprite>("Icon/icon_map/" + model.id);

        SellectLevelManager.SetLevelCurrent(data.levelStart);
        SellectLevelManager.SetIdMap(idMap);
    }
}
