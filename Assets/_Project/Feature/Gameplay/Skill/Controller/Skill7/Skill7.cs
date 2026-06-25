using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Skill7 : SkillBaseController
{
    protected override Vector2 GetDirection(Vector3 posTo, Vector3 posFrom)
    {
        var targetTo = skillDetails.mechanicType.First(x => x.mechanicTypes == EnumBase.MechanicTypes.TargetTo);

        if (Convert.ToInt32(targetTo.values) == 3)
        {
            return BattleController.instance.joystick.Direction();
        }

        Vector2 dir = ((Vector2)posTo - (Vector2)posFrom);

        // Không có target
        if (dir.sqrMagnitude < 0.001f)
        {
            float angle = UnityEngine.Random.Range(0f, 360f) * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }

        return dir.normalized;
    }
}
