using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
