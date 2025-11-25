using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BattleManager
{
    public static EnumBase.BattleMode battleMode;

    public static void SetBattleMode(EnumBase.BattleMode battle) {  battleMode = battle; }
    public static EnumBase.BattleMode GetBattleMode() => battleMode;
}
