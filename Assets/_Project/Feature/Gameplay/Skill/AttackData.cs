using System;
using System.Collections.Generic;

[Serializable]
public class AttackData
{
    public Dictionary<EnumBase.RPGStatType, float> stats = new ();
    public Dictionary<EnumBase.EffectType, float[]> effects = new ();

    public AttackData() { }

    public AttackData(AttackData other)
    {
        if (other == null) return;

        foreach (var item in other.stats)
            stats[item.Key] = item.Value;

        foreach (var item in other.effects)
            effects[item.Key] = item.Value == null ? null : (float[])item.Value.Clone();
    }
}
