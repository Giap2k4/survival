using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleController : SingletonTemporary<BattleController>
{
    [SerializeField]
    public CinemachineVirtualCamera cam;

    [SerializeField]
    public Joystick joystick;

    [SerializeField]
    public Camera mainCamera;

    private static GameObject _player;

    // Start is called before the first frame update
    void Start()
    {
        if (BattleManager.BattleMode == EnumBase.BattleMode.None) BattleManager.BattleMode = (EnumBase.BattleMode.LastSurvival);

        switch (BattleManager.BattleMode)
        {
            case EnumBase.BattleMode.LastSurvival:
                var obj = gameObject.GetOrAddComponent<BattleLastSurvivalMode>();
                break;
        }
    }

    public void SetPlayer(GameObject obj) => _player = obj;
    public GameObject GetPlayer() => _player;
}
