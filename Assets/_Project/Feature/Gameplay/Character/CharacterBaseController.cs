using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBaseController : MonoBehaviour
{
    public RPGStatCollection stats;

    public void AddStatBase(int idStat)
    {
        if (stats == null)
        {
            stats = new RPGStatCollection();
        }
        

        var stat = DataManager.Stats.GetStatsById(idStat);
        foreach (var item in stat.statDetail)
        {
            stats.AddModifier(item.statType, new StatModifier(item.valueType, item.statValue, "base"));
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
