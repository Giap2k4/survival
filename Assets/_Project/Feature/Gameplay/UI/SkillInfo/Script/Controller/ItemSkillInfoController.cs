using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSkillInfoController : MonoBehaviour
{
    public static event Action eventClick;
    [SerializeField]
    protected Button btnClick;

    [SerializeField]
    protected int idSkillObj;

    [SerializeField]
    protected Image image;

    [SerializeField]
    protected TextMeshProUGUI txtLevel;

    [SerializeField]
    protected TextMeshProUGUI txtSkillName;

    [SerializeField]
    protected TextMeshProUGUI txtSkillDescription;

    private void Start()
    {
        btnClick.onClick.AddListener(OnClick);
    }

    public void SetData(int idSkill)
    {
        var skill = DataManager.SkillInfo.GetSkillById(idSkill);

        idSkillObj = idSkill;
        image.sprite = Resources.Load<Sprite>("Skill/" + idSkill);
        var skillOwn = SkillManager.instance.GetAllSkill().FirstOrDefault(x => x.GetIdSkill() == idSkill);
        if (skillOwn == null) txtLevel.text = "Mới";
        else txtLevel.text = "Cấp " + (skillOwn.GetLevelCurrent() + 1);
        txtSkillName.text = skill.name;
        txtSkillDescription.text = skill.description;
    }

    public void OnClick()
    {
        SkillManager.instance.SetOrAddSkill(idSkillObj);
        eventClick?.Invoke();
    }
}