using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill5 : SkillBaseController
{
    protected override Vector3 GetTargetFrom()
    {
        var cam = BattleController.instance.mainCamera;
        float vy = 1.1f;
        float vx = 0.5f;
        float z = Mathf.Abs(cam.transform.position.z);

        Vector3 posFrom = cam.ViewportToWorldPoint(new Vector3(vx, vy, z));
        return posFrom;
    }
}
