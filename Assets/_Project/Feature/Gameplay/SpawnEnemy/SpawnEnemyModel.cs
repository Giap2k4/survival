using JetBrains.Annotations;
using System;
using System.Net.Security;

[Serializable]
public class SpawnEnemyModel
{
    public EnumBase.BattleMode battleMode;
    public BattleModeDetails[] battleModeDetail;
}

[Serializable]
public class BattleModeDetails
{
    public int idMap;
    public int totalTime; // tính bằng giây
    public SpawnEnemyDetails[] details;
}

[Serializable]
public class SpawnEnemyDetails
{
    public int idEnemy;
    public int levelEnemy;
    public int quantityEnemy;
    public EnumBase.SpawnEnemyType spawnEnemyType;
    public float timeStart;
    public float timeEnd;
    public float interval;
}
