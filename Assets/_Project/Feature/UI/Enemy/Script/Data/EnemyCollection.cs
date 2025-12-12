using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class EnemyCollection : ScriptableObject
{
    public EnemyModel[] dataGroups;

    public EnemyModel GetEnemyById(int id) => dataGroups.First(x => x.id == id);
}
