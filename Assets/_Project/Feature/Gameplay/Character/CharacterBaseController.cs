using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBaseController : MonoBehaviour
{
    public RPGStatCollection stats;

    protected virtual void OnEnable() { }

    public void AddStatBase(int idStat, int level)
    {
        if (stats == null) stats = new RPGStatCollection();

        var stat = DataManager.Stats.GetStatsById(idStat);
        foreach (var item in stat.statDetail)
        {
            // tính level
            var value = item.statValue;
            value = FormulaEvaluator.Evaluate(stat.formula, value, item.bonusValue, level);

            stats.SetValueBase(item.statType, value);
        }

        SetData();
    }

    public virtual void OnTriggerEnter2D(Collider2D collision) { }

    public virtual void OnCollisionEnter2D(Collision2D collision) { }
    public virtual void OnCollisionStay2D(Collision2D collision) { }

    public virtual void InitSkillDefault(int idSkill)
    {
        var skill = Resources.Load<GameObject>("SkillController_" + idSkill);
        GameObject obj = Instantiate(skill);
        obj.transform.position = Vector3.zero;
    }

    public virtual void TakeDamage(AttackData data) { }

    /// <summary>
    /// Set data khi đã lấy được các chỉ số stat (Hp,...)
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
