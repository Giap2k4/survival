using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSkillOwnController : MonoBehaviour
{
    [SerializeField]
    protected Image image;
    [SerializeField]
    protected TextMeshProUGUI txtLevel;
    public void SetData(int idSkill, int levelSkill) 
    {
        var sprite = Resources.Load<Sprite>("Skill/" + idSkill);
        image.sprite = sprite;
        txtLevel.text = "Cấp " + levelSkill.ToString();
    }
}
