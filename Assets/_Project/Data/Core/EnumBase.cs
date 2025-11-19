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
}
