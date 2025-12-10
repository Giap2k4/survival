using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLastSurvivalMode : BattleModeBase
{
    protected override void InitData()
    {
        InitHero();
    }

    protected void InitHero()
    {
        int idHero = PlayerDataManager.Hero.database.idHeroSelected;
        var prefab = Resources.Load<GameObject>("Hero_" + idHero);
        GameObject obj = Instantiate(prefab);
        obj.transform.position = Vector3.zero;

        BattleController.instance.SetPlayer(obj);

        obj.GetComponent<CharacterMovement>().joystick = BattleController.instance.joystick;
        BattleController.instance.cam.Follow = obj.transform;
    }
}
