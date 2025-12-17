using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill1 : SkillBaseController
{
    protected override void BeforeSpawnProjectile()
    {
        base.BeforeSpawnProjectile();
        var parent = BattleController.instance.GetPlayer().transform;

        parentProjectile = parent;
    }

    protected override int? GetProjectileNumberModify() => 1;

    protected override void InitDataProjectile(ProjectileModel data, GameObject projectile)
    {
        var component = projectile.GetComponent<ProjectileBaseController>();

        data.projectileNumber = GetProjectileNumber();

        component.InitData(data, this);
    }
}
