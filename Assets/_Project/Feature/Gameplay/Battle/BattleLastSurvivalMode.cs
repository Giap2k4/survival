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
        obj.transform.position = Vector3.zero;

        Debug.Log(joystick);
        obj.GetComponent<CharacterMovement>().joystick = joystick;
        cam.Follow = obj.transform;
    }
}
