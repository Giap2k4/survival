using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    [SerializeField]
    protected CinemachineVirtualCamera cam;

    [SerializeField]
    protected Joystick joystick;

    // Start is called before the first frame update
    void Start()
    {
        if (BattleManager.battleMode == EnumBase.BattleMode.None)
        {
            BattleManager.SetBattleMode(EnumBase.BattleMode.LastSurvival);
        }

        switch (BattleManager.GetBattleMode())
        {
            case EnumBase.BattleMode.LastSurvival:
                var obj = gameObject.GetOrAddComponent<BattleLastSurvivalMode>();
                obj.cam = cam;
                obj.joystick = joystick;
                break;
        }
    }
}
