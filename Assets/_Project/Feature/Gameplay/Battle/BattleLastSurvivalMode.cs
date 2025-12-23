using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLastSurvivalMode : BattleModeBase
{
    protected override void InitData()
    {
        InitHero();
        SpawnEnemy();
    }

}
