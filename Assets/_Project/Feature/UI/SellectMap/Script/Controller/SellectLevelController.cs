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

    [SerializeField]
    protected Button btnLeft;

    [SerializeField]
    protected Button btnRight;

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
        //PlayerDataManager.SellectLevel.Clear();
        var levelCurrent = SellectLevelManager.GetLevelCurrent();
        var idMap = SellectLevelManager.GetIdMap();

        txtNameMap.text = levelCurrent.ToString() + ". " + SellectLevelManager.GetNameMap(idMap);
        imageSellectMap.sprite = Resources.Load<Sprite>("Icon/icon_map/" + idMap);

        btnLeft.onClick.AddListener(OnClickLeft);
        btnRight.onClick.AddListener(OnClickRight);
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

    public void OnClickLeft()
    {
        int level = SellectLevelManager.GetLevelCurrent();
        level = Mathf.Max(1, level - 1);
        SellectLevelManager.SetLevelCurrent(level);

        int idMap = SellectLevelManager.GetIdMap();
        idMap = Mathf.Max(1, idMap);

        var map = DataManager.SellectMap.GetById(idMap);

        if (level < map.levelStart && idMap > 1)
        {
            idMap--;
            SellectLevelManager.SetIdMap(idMap);
            map = DataManager.SellectMap.GetById(idMap);

            imageSellectMap.sprite = Resources.Load<Sprite>($"Icon/icon_map/{idMap}");
        }

        txtNameMap.text = $"{level}. {map.nameMap}";
    }


    public void OnClickRight()
    {
        int level = SellectLevelManager.GetLevelCurrent() + 1;
        if (level > SellectLevelManager.GetLevelTotal()) return;

        SellectLevelManager.SetLevelCurrent(level);

        int idMap = SellectLevelManager.GetIdMap();
        idMap = Mathf.Max(1, idMap);

        var map = DataManager.SellectMap.GetById(idMap);
        if (map == null) return;

        var nextMap = DataManager.SellectMap.GetById(idMap + 1);

        if (nextMap != null && level >= nextMap.levelStart)
        {
            idMap++;
            SellectLevelManager.SetIdMap(idMap);
            map = nextMap;

            imageSellectMap.sprite = Resources.Load<Sprite>($"Icon/icon_map/{idMap}");
        }

        txtNameMap.text = $"{level}. {map.nameMap}";
    }

}
