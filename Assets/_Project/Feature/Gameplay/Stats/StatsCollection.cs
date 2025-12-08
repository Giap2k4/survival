using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatsCollection : ScriptableObject
{
    public StatsModel[] dataGroups;

    public StatsModel GetStatsById(int id) => dataGroups.First(x => x.idStat == id);
}
