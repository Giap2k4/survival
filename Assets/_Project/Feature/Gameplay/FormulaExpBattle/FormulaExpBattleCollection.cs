using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FormulaExpBattleCollection : ScriptableObject
{
    public FormulaExpBattleModel[] dataGroups;

    public FormulaExpBattleModel GetByBattleMode(EnumBase.BattleMode battleMode) => dataGroups.First(x => x.battleMode == battleMode);
}
