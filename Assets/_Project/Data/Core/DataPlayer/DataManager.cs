using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    public static Dictionary<string, Object> cacheDataConfig = new Dictionary<string, Object>();

    public static T Get<T> () where T : Object
    {
        var key = typeof(T).Name;
        if (cacheDataConfig.TryGetValue(key, out var value))
        {
            return (T)value;
        }

        var data = Resources.Load<T>(key);
        cacheDataConfig.Add(key, data);
        return data;
    }

    public static HeroCollection Hero => Get<HeroCollection>();
    public static ResourceCollection Resource => Get<ResourceCollection>();
    public static StatsCollection Stats => Get<StatsCollection>();
    public static SpawnEnemyCollection SpawnEnemy => Get<SpawnEnemyCollection>();
    public static EnemyCollection Enemy => Get<EnemyCollection>();
    public static SkillCollection Skill => Get<SkillCollection>();
    public static FormulaExpBattleCollection FormulaExpBattle => Get<FormulaExpBattleCollection>();
    public static ExpBattleCollection ExpBattle => Get<ExpBattleCollection>();
    public static SkillInfoCollection SkillInfo => Get<SkillInfoCollection>();
    public static RewardCollection Reward => Get<RewardCollection>();
    public static SellectMapCollection SellectMap => Get<SellectMapCollection>();
    public static EquipmentCollection Equipment => Get<EquipmentCollection>();
    public static GrowthFundCollection GrowthFund => Get<GrowthFundCollection>();
    public static SevenDayLoginCollection SevenDayLogin => Get<SevenDayLoginCollection>();
}
