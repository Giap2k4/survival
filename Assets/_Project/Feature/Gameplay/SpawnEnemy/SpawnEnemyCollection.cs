using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class SpawnEnemyCollection : ScriptableObject
{
    public SpawnEnemyModel[] dataGroups;

    public SpawnEnemyModel GetSpawnEnemyById(int id) => dataGroups.First(x => x.idMap == id);
}
