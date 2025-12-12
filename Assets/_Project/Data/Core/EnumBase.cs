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
        Main
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
        Damager = 2,
        MoveSpeed = 3,
        CritRate = 4,
        CritDamager = 5,
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
}
