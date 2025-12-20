using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill3 : SkillBaseController
{
    protected int numberProjectile;
    protected int countProjectile;

    protected float GetRotateDistance()
    {
        return 360/ (float)GetProjectileNumber();
    }

    protected override Quaternion GetRotate(Vector3 posTo, Vector3 posFrom)
    {
        var rotate = base.GetRotate(posTo, posFrom);

        float angle = rotate.eulerAngles.z;
        if (countProjectile > 0)
        {
            angle += countProjectile * GetRotateDistance();
        } 

        countProjectile++;

        return Quaternion.Euler(0, 0, angle);
    }

    protected override void BeforeSpawnProjectile()
    {
        countProjectile = 0;
    }
}
