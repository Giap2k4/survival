using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLastSurvivalMode : BattleModeBase
{
    protected override void InitData()
    {
        int idHero = PlayerDataManager.Hero.database.idHeroSelected;
        var prefab = Resources.Load<GameObject>("Hero_" + idHero);
        GameObject obj = Instantiate(prefab);
    }
}
