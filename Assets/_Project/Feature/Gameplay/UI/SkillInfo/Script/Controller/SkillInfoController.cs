using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SkillInfoController : FeatureBaseController
{
    [Header("Cấu hình riêng")]
    [SerializeField]
    protected Transform parentObj;

    [SerializeField]
    protected GameObject item;

    [SerializeField]
    protected Canvas canvas;

    protected List<GameObject> listItem = new();

    protected int quantityLevel; // lên mấy level 1 lần
    protected int startQuantityLevel;
    protected int countLevel;
    [SerializeField]
    protected TextMeshProUGUI txtCountLevel;

    [SerializeField]
    protected GameObject skillOwn;
    [SerializeField]
    protected Transform parentSkillOwn;

    protected override void Start()
    {
        base.Start();
        canvas.sortingLayerName = "UI";
        canvas.sortingOrder = 200;

        foreach (var item in SkillManager.instance.GetAllSkill())
        {
            GameObject obj = Instantiate(skillOwn, parentSkillOwn);
            obj.GetComponent<ItemSkillOwnController>().SetData(item.GetIdSkill(), item.GetLevelCurrent());
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        countLevel = 0;
        startQuantityLevel = 0;
        ItemSkillInfoController.eventClick += HandleLevelUp;
    }

    protected override void OnDisable()
    {
        ItemSkillInfoController.eventClick -= HandleLevelUp;
        if (SkillManager.instance == null) return;
        SkillManager.instance.HanldeSkillLevelUP();
    }

    public void SetQuantityLevel(float number)
    {
        quantityLevel = (int)number;
        startQuantityLevel = quantityLevel;
        var data = DataManager.SkillInfo.GetSharedSkill();

        HandleLevelUp();
    }

    public void HandleLevelUp()
    {
        countLevel++;
        txtCountLevel.text = countLevel.ToString() + "/" + startQuantityLevel.ToString();
        if (quantityLevel <= 0)
        {
            LevelExpController.instance.TimeScale();
            UIManager.instance.CloseFeature(feature);
            return;
        }

        if (SkillManager.instance.IsAllSkillMaxLevel())
        {
            LevelExpController.instance.TimeScale();
            UIManager.instance.CloseFeature(feature);
            return;
        }

        foreach (var item in listItem)
        {
            item.SetActive(false);
        }
        listItem.Clear();

        var list = SkillManager.instance.GetSkillRandom().ToList();
        int spawnCount = Mathf.Min(3, list.Count);

        for (int i = 0; i < spawnCount; i++)
        {
            var random = Random.Range(0, list.Count);
            var idSkill = list[random];

            GameObject obj = Instantiate(item, parentObj);
            obj.GetComponent<ItemSkillInfoController>().SetData(idSkill);
            listItem.Add(obj);
            list.RemoveAt(random);
        }

        quantityLevel--;
    }
}
