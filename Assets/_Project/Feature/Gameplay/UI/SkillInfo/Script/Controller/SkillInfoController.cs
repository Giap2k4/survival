using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    protected override void Start()
    {
        base.Start();
        canvas.sortingLayerName = "UI";
        canvas.sortingOrder = 200;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
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
        var data = DataManager.SkillInfo.GetSharedSkill();

        HandleLevelUp();
    }

    /// <summary>
    /// random skill và spawn item
    /// </summary>
    public void HandleLevelUp()
    {
        if (quantityLevel <= 0)
        {
            UIManager.instance.CloseFeature(feature);
            LevelExpController.instance.SetTimeScale();
            return;
        }

        foreach (var item in listItem)
        {
            item.SetActive(false);
        }
        listItem.Clear();

        // random
        var list = SkillManager.instance.GetSkillRandom().ToList();

        for (int i = 0; i < 3; i++)
        {
            var random = Random.Range(0, list.Count - 1);
            var idSkill = list[random];

            GameObject obj = Instantiate(item, parentObj);
            obj.GetComponent<ItemSkillInfoController>().SetData(idSkill);
            listItem.Add(obj);
            list.RemoveAt(random);
        }

        quantityLevel--;
    }
}
