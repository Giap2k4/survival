using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : CharacterBaseController
{
    [SerializeField]
    protected float hp;

    [SerializeField]
    protected float expCurrent; // số lượng exp

    [SerializeField]
    protected float levelExp; // level exp hiện tại

    protected float levelExpStart; // level exp trước khi tăng level
    protected float numberSidebar;
    protected FormulaExpBattleModel model;

    protected override void SetData()
    {
        base.SetData();
        hp = stats.GetOrCreateStat(EnumBase.RPGStatType.Health).valueStat;
        levelExp = 1;
        expCurrent = 0;
        model = DataManager.FormulaExpBattle.GetByBattleMode(BattleManager.BattleMode);        
    }

    public void AddExp(int value)
    {
        expCurrent += value;
        levelExpStart = levelExp;
        CaculaterExp();
    }

    protected void CaculaterExp()
    {
        var levelTemp = FormulaEvaluator.EvaluateLevel(expCurrent, model.valueBase, model.bonus);

        levelExp = levelTemp;
        var levelUp = levelExp - levelExpStart;
        HandleLevelUp(levelUp);
        levelExpStart = levelExp;

        if (levelExp <= 0)
        {
            levelExp = 1;
            numberSidebar = expCurrent;
            return;
        }
        numberSidebar = expCurrent - FormulaEvaluator.Evaluate(model.formula, model.valueBase, model.bonus, levelExp);
    }

    protected void HandleLevelUp(float numberLevel)
    {
        // numberLevel: tăng mấy level
        if (numberLevel <= 0) return;

        // xử lý tăng level
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            var dmgAttack = collision.gameObject.GetComponent<EnemyController>().GetDamageAttack();
            HandleDamageAttack(dmgAttack);
        }
    }

    protected void HandleDamageAttack(float dmg)
    {
        hp -= dmg;
        if (hp <= 0) hp = 0;

    }

    protected void UpdateUI()
    {

    }
}
