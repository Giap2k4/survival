using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroDetailController : FeatureBaseController
{
    [Header("Cấu hình riêng")]
    [SerializeField]
    protected Image avatarHero;

    [SerializeField]
    protected TextMeshProUGUI txtNameHero;

    [SerializeField]
    protected Image avatarSkillDefault;

    [SerializeField]
    protected TextMeshProUGUI descSkillDefault;

    [SerializeField]
    protected GameObject upgrade;

    [SerializeField]
    protected GameObject buy;

    [SerializeField]
    protected GameObject txtComingSoon;

    public void SetData(HeroModel heroModel)
    {
        txtNameHero.text = heroModel.nameHero;
        avatarHero.sprite = Resources.Load<Sprite>("Hero/" + heroModel.id);
        avatarSkillDefault.sprite = Resources.Load<Sprite>("Skill/" + heroModel.defaultSkill);
        descSkillDefault.text = DataManager.SkillInfo.GetSkillById(heroModel.defaultSkill).description;

        if (HeroManager.ListHeroOwned().ContainsKey(heroModel.id))
        {
            upgrade.SetActive(true);
        } else
        {
            if (DataManager.Hero.GetHeroById(heroModel.id).resource.resQuantity == 0) txtComingSoon.SetActive(true);
            else buy.SetActive(true);
        }
    }
}
