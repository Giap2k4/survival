using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill2 : SkillBaseController
{
    protected int number = 0;

    protected override void BeforeSpawnProjectile()
    {
        number = 0;
    }

    protected override Quaternion GetRotate(Vector3 posTo, Vector3 posFrom)
    {
        var rotate = base.GetRotate(posTo, posFrom);
        float angle = rotate.eulerAngles.z;

        if (number == 1) angle += HandleCustomValue1(); 
        else if (number == 2) angle -= HandleCustomValue1();
        number ++;

        return Quaternion.Euler(0, 0, angle);
    }
}
