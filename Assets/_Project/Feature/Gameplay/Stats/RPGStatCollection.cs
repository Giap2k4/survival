using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RPGStatCollection
{
    private Dictionary<EnumBase.RPGStatType, RPGStat> _stats = new Dictionary<EnumBase.RPGStatType, RPGStat> ();

    public RPGStat GetOrCreateStat(EnumBase.RPGStatType statType)
    {
        if (!_stats.ContainsKey(statType))
        {
            _stats.Add (statType, new RPGStat ());
        }
        return _stats[statType];
    }

    public void SetValueBase(EnumBase.RPGStatType statType, float valueStat)
    {
        GetOrCreateStat(statType).SetValueStatBase(valueStat);
    }

    public void AddModifier(EnumBase.RPGStatType statType, StatModifier statMod)
    {
        GetOrCreateStat(statType).AddModifier(statMod);
    }

    public void RemoveModifier(EnumBase.RPGStatType statType, StatModifier statMod)
    {
        if (_stats.ContainsKey(statType))
        {
            _stats[statType].RemoveModifier(statMod);
        }
    }
}
