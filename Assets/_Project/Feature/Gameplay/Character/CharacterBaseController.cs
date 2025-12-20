using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBaseController : MonoBehaviour
{
    public RPGStatCollection stats;

    protected virtual void OnEnable()
    {

    }

    public void AddStatBase(int idStat)
    {
        if (stats == null)
        {
            stats = new RPGStatCollection();
        }
        

        var stat = DataManager.Stats.GetStatsById(idStat);
        foreach (var item in stat.statDetail)
        {
            //stats.AddModifier(item.statType, new StatModifier(item.valueType, item.statValue, "base"));
            stats.SetValueBase(item.statType, item.statValue);
        }

        SetData();
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    public virtual void InitSkillDefault(int idSkill)
    {
        var skill = Resources.Load<GameObject>("SkillController_" + idSkill);
        GameObject obj = Instantiate(skill);
        obj.transform.position = Vector3.zero;
    }

    public virtual void TakeDamage(AttackData data) { }

    /// <summary>
    /// Set data khi đã lấy được các chỉ số stat
    /// </summary>
    protected virtual void SetData() { }

    /// <summary>
    /// Set level của character
    /// </summary>
    /// <param name="level"></param>
    public virtual void SetLevel(int level) { }

    /// <summary>
    /// Hàm check HP
    /// </summary>
    protected virtual void IsDead(float dmg)
    {

    }
}
