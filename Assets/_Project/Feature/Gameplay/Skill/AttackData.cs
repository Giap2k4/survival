using System;
using System.Collections.Generic;

[Serializable]
public class AttackData
{
    public Dictionary<EnumBase.RPGStatType, float> stats = new Dictionary<EnumBase.RPGStatType, float>();
    public Dictionary<EnumBase.EffectType, float[]> effects = new Dictionary<EnumBase.EffectType, float[]>();
}
