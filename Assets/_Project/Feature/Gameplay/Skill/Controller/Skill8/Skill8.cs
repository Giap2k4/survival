using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill8 : SkillBaseController
{
    protected override void BeforeSpawnProjectile()
    {
        base.BeforeSpawnProjectile();
        var parent = BattleController.instance.GetPlayer().transform;

        parentProjectile = parent;
    }
}
