using System;

[Serializable]
public class StatsModel
{
    public int idStat;
    public StatDetail[] statDetail;
}

[Serializable]
public class StatDetail
{
    public EnumBase.RPGStatType statType;
    public EnumBase.ValueType valueType;
    public float statValue;
}
