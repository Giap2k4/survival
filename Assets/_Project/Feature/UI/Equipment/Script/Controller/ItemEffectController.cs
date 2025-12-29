using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemEffectController : MonoBehaviour
{
    [SerializeField]
    protected Sprite spriteHp;

    [SerializeField]
    protected Sprite spriteDmg;

    [SerializeField]
    protected Image image;

    [SerializeField]
    protected TextMeshProUGUI txtEff;

    public void SetData(EquipmentDetail equip, int idEquip)
    {
        switch(equip.statType)
        {
            case EnumBase.RPGStatType.Damage:
            case EnumBase.RPGStatType.Cooldown:
                image.sprite = spriteDmg;
                break;
            case EnumBase.RPGStatType.Health:
            case EnumBase.RPGStatType.HealthRegen:
                image.sprite = spriteHp;
                break;
        }

        var value = FormulaEvaluator.ConvertStringToFloat(equip.value);

        if (value[0] == 1) txtEff.text = "+1%" + "(+" + value[1] * 100 + "%) " + equip.description;
        else txtEff.text = "+1" + "(+" + value[1] + ") " + equip.description;

        if (equip.statType == EnumBase.RPGStatType.Cooldown) txtEff.text = "-1%" + "(-" + value[1] * 100 + "%) " + equip.description;
    }
}
