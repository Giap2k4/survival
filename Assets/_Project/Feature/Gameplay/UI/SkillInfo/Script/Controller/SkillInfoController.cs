using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInfoController : FeatureBaseController
{
    [Header("Cấu hình riêng")]
    [SerializeField]
    protected Transform parentObj;

    [SerializeField]
    protected GameObject item;

    protected int quantityLevel; // lên mấy level 1 lần

    public void SetQuantityLevel(int number)
    {
        quantityLevel = number;
        var data = DataManager.SkillInfo.GetSharedSkill();

        // tạo random cho số lượng level up
    }
}
