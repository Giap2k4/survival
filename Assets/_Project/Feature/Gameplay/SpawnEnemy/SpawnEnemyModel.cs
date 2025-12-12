using System;

[Serializable]
public class SpawnEnemyModel
{
    public int idMap;
    public SpawnEnemyDetails[] details;
}

[Serializable]
public class SpawnEnemyDetails
{
    public int idEnemy;
    public int quantityEnemy;
    public EnumBase.SpawnEnemyType spawnEnemyType;
    public float timeStart;
    public float timeEnd;
    public float interval;
}
