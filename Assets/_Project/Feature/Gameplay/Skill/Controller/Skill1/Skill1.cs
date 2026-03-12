using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Skill1 : SkillBaseController
{
    [SerializeField]
    protected Projectile1 obj; 
    protected override void BeforeSpawnProjectile()
    {
        base.BeforeSpawnProjectile();
        var parent = BattleController.instance.GetPlayer().transform;

        parentProjectile = parent;
    }

    protected override int? GetProjectileNumberModify() => 1;

    protected override GameObject InitDataProjectile(ProjectileModel data, GameObject projectile)
    {
        var component = projectile.GetComponent<ProjectileBaseController>();

        data.projectileNumber = GetProjectileNumber();

        component.InitData(data, this);
        obj = projectile.GetComponent<Projectile1>();
        return projectile;
    }

    public override void HandleLevelUP()
    {
        if (skillModel == null)
        {
            string nameSkill = "Skill_" + idSkill;
            skillModel = Resources.Load<SkillCollection>(nameSkill).dataGroups;
        }

        if (characterBaseController == null) characterBaseController = BattleController.instance.GetPlayer().GetComponent<CharacterBaseController>();

        skillDetails = skillModel.details.First(x => x.level == levelCurrent);
        cooldown = skillDetails.mechanicType.FirstOrDefault(x => x.mechanicTypes == EnumBase.MechanicTypes.Cooldown);
        checkLevelUp = true;

        InitAttackData();

        if (obj == null) return;

        var data = InitProjectileData();
        data.projectileNumber = GetProjectileNumber();
        obj.SetAttackData(attackData);
        obj.InitData(data, this);
    }
}
