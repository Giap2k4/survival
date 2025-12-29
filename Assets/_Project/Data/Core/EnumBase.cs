using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumBase
{
    public enum Feature
    {
        None,
        Hero,
        Equipment,
        Pet,
        Shop, 
        Main,
        SellectMap,

        SkillInfo,
        PauseGame,
        Reward,
        HeroDetail,
    }

    public enum Scenes
    {
        LoadScene,
        HomeScene,
        BattleScene
    }

    public enum ResourcesType
    {
        Money,
        Hero
    }

    public enum BattleMode
    {
        None,
        LastSurvival
    }

    public enum RPGStatType
    {
        None,
        Health = 1,
        Damage = 2,
        MoveSpeed = 3,
        CritRate = 4,
        CritDamage = 5,
    }

    public enum EffectType
    {
        None = 0,
        KnockBack = 1,    // Đẩy lùi
        Freeze = 2,       // Đóng băng
        Poison = 3,       // Độc
        Burn = 4,         // Đốt cháy
        Pierce = 5,      // Xuyên qua quái
        Slow = 6,        // Làm chậm
        Stun = 7,        // Choáng
    }

    public enum ValueType
    {
        None = 0,
        Percent = 1,
        Number = 2,
    }

    public enum SpawnEnemyType
    {
        None = 0,
        Top = 1,
        Bot = 2,
        Left = 3,
        Right = 4,
        All = 5,
    }

    public enum MechanicTypes
    {
        Cooldown = 1,
        Duration = 2,
        Damage = 3,
        Range = 4,
        DetectRange = 5,
        ProjectileNumber = 6,
        ProjectileSpeed = 7,
        ProjectileSize = 8,
        TargetFrom = 9,
        TargetTo = 10,
        FireRate = 11, // Khoảng cách thời gian bắn giữa các viên đạn

        // các eff
        effect_1,
        effect_2,
        effect_3,

        // custom value
        custom_value_1,
        custom_value_2,
    }
}
