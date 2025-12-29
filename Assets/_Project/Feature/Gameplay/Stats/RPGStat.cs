using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RPGStat
{
    public float statValueBase;
    private List<StatModifier> _statModifiers = new List<StatModifier>();
    private float _modStatValue;

    public float valueStat => statValueBase + _modStatValue;

    public void SetValueStatBase (float value) => statValueBase = value;

    public void AddModifier(StatModifier mod)
    {
        _statModifiers.Add(mod);
        UpdateModifiers();
    }

    public void RemoveModifier(StatModifier mod)
    {
        _statModifiers.Remove(mod);
        UpdateModifiers();
    }

    public void ClearAllMod()
    {
        _statModifiers.Clear();
        UpdateModifiers();
    }

    public void UpdateModifiers()
    {
        _modStatValue = 0;
        // Hiện tại tính theo % thì chỉ tính % của base chứ k tính theo % của tổng đã được cộng từ nhiều nơi
        foreach (var item in _statModifiers)
        {
            if (item.valueType == EnumBase.ValueType.Percent)
            {
                _modStatValue = statValueBase + (item.statValue * statValueBase);
            } else
            {
                _modStatValue = statValueBase + item.statValue;
            }
        }
    }
    
}

public class StatModifier
{
    public EnumBase.ValueType valueType;
    public float statValue;
    public string source;

    public StatModifier (EnumBase.ValueType valueType, float statValue, string source)
    {
        this.valueType = valueType;
        this.statValue = statValue;
        this.source = source;
    }
}
