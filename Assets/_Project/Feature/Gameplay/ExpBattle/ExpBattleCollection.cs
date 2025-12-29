using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExpBattleCollection : ScriptableObject
{
    public ExpBattleModel[] dataGroups;

    public ExpBattleModel GetExpBattleById(int id) => dataGroups.First(x => x.typeExp == id);
}
