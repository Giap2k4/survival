using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class SpawnEnemyCollection : ScriptableObject
{
    public SpawnEnemyModel[] dataGroups;

    public BattleModeDetails GetSpawnEnemyById(int id)
    {
        var obj = dataGroups.First(x => x.battleMode == BattleManager.BattleMode).battleModeDetail;
        var item = obj.First(x => x.idMap == id);
        return item;
    }
}
