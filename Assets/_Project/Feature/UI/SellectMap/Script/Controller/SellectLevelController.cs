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
        var levelNext = SellectLevelManager.GetLevelCurrent();
        levelNext--;

        if (levelNext <= 0)
        {
            levelNext = 1;
            SellectLevelManager.SetLevelCurrent(levelNext);
            return;
        }

        SellectLevelManager.SetLevelCurrent(levelNext);
        var idMap = SellectLevelManager.GetIdMap();
        var data = DataManager.SellectMap.GetById(idMap);
        txtNameMap.text = levelNext.ToString() + ". " + DataManager.SellectMap.GetById(idMap).nameMap;

        if (idMap - 1 <= 0)
        {
            idMap = 1;
            txtNameMap.text = levelNext.ToString() + ". " + DataManager.SellectMap.GetById(idMap).nameMap;
        } 
        

        if (levelNext < data.levelStart)
        {
            idMap--;
            SellectLevelManager.SetIdMap(idMap);
            imageSellectMap.sprite = Resources.Load<Sprite>("Icon/icon_map/" + idMap);
            txtNameMap.text = levelNext.ToString() + ". " + DataManager.SellectMap.GetById(idMap).nameMap;
        }
    }

    public void OnClickRight()
    {
        var levelNext = SellectLevelManager.GetLevelCurrent();
        levelNext++;

        if (levelNext > SellectLevelManager.GetLevelTotal()) return;

        // kiểm tra xem đã đến được leve này chưa

        SellectLevelManager.SetLevelCurrent(levelNext);
        var idMap = SellectLevelManager.GetIdMap();
        var data = DataManager.SellectMap.GetById(idMap + 1);
        txtNameMap.text = levelNext.ToString() + ". " + DataManager.SellectMap.GetById(idMap).nameMap;

        if (data == null)
        {
            data = DataManager.SellectMap.GetById(idMap);
            if (levelNext >= data.levelStart)
            {
                SellectLevelManager.SetIdMap(idMap);
                imageSellectMap.sprite = Resources.Load<Sprite>("Icon/icon_map/" + idMap);
                txtNameMap.text = levelNext.ToString() + ". " + DataManager.SellectMap.GetById(idMap).nameMap;
            }
            return;
        }
        

        if (levelNext >= data.levelStart)
        {
            idMap++;
            SellectLevelManager.SetIdMap(idMap);
            imageSellectMap.sprite = Resources.Load<Sprite>("Icon/icon_map/" + idMap);
            txtNameMap.text = levelNext.ToString() + ". " + DataManager.SellectMap.GetById(idMap).nameMap;
        }
    }
}
