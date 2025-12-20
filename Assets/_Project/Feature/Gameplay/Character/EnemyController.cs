using System;
using UnityEngine;

public class EnemyController : CharacterBaseController
{
    protected int level;

    [SerializeField]
    protected float hp;

    [SerializeField]
    protected float damage;

    protected AttackData attackData;

    protected override void OnEnable()
    {
        base.OnEnable();
        // add modify stat nếu có level > 1
        //SetData();
    }

    public override void SetLevel(int level) => this.level = level;

    protected override void SetData()
    {
        hp = stats.GetOrCreateStat(EnumBase.RPGStatType.Health).valueStat;
        damage = stats.GetOrCreateStat(EnumBase.RPGStatType.Damage).valueStat;
    }

    public AttackData GetAttackData() => attackData;

    public override void TakeDamage(AttackData data)
    {
        // Xử lý các value stat
        HandleValuesStats(data);
        // Xử lý các eff
        HandleEffect(data);
    }

    /// <summary>
    /// Xử lý các giá trị stat
    /// </summary>
    /// <param name="data"></param>
    protected virtual void HandleValuesStats(AttackData data)
    {
        float dmg = data.stats[EnumBase.RPGStatType.Damage];
        float critDmg = data.stats[EnumBase.RPGStatType.CritDamage];
        float critRate = data.stats[EnumBase.RPGStatType.CritRate];

        bool isCrit = UnityEngine.Random.Range(0, 101) < critRate * 100;

        if (isCrit) dmg += critDmg * dmg;
        IsDead(dmg);
        UpdateUI(dmg, isCrit);

    }

    /// <summary>
    /// Xử lý các eff nhận được
    /// </summary>
    /// <param name="data"></param>
    protected virtual void HandleEffect(AttackData data)
    {
        foreach (var item in data.effects)
        {
            switch ((EnumBase.EffectType)item.Value[0])
            {
                case EnumBase.EffectType.KnockBack:
                    HandleEffectKnockBack();
                    break;

                case EnumBase.EffectType.Freeze:
                    HandleEffectFreeze();
                    break;

                case EnumBase.EffectType.Poison:
                    HandleEffectPoison();
                    break;

                case EnumBase.EffectType.Burn:
                    HandleEffectBurn();
                    break;

                case EnumBase.EffectType.Pierce:
                    HandleEffectPierce();
                    break;

                case EnumBase.EffectType.Slow:
                    HandleEffectSlow();
                    break;

                case EnumBase.EffectType.Stun:
                    HandleEffectStun();
                    break;
            }
        }
    }

    /// <summary>
    /// xử lý eff đẩy lùi
    /// </summary>
    protected virtual void HandleEffectKnockBack() { }

    /// <summary>
    /// xử lý eff đóng băng
    /// </summary>
    protected virtual void HandleEffectFreeze() { }

    /// <summary>
    /// xử lý eff độc
    /// </summary>
    protected virtual void HandleEffectPoison() { }

    /// <summary>
    /// xử lý eff đốt cháy
    /// </summary>
    protected virtual void HandleEffectBurn() { }

    /// <summary>
    /// xử lý eff xuyên qua quái
    /// </summary>
    protected virtual void HandleEffectPierce() { }

    /// <summary>
    /// Xử lý eff làm chậm
    /// </summary>
    protected virtual void HandleEffectSlow() { }

    /// <summary>
    /// xử lý eff choáng
    /// </summary>
    protected virtual void HandleEffectStun() { }

    protected override void IsDead(float dmg)
    {
        base.IsDead(dmg);
        hp -= dmg;
        if (hp < 0)
        {
            hp = 0;
            gameObject.SetActive(false);
        }
    }

    protected void UpdateUI(float dmg, bool isCrit)
    {
        // bật thanh hp, hiện text HP bị trừ
        GameObject obj = DamageTextManager.instance.DamageText(transform.position);

        obj.GetComponent<DamageTextController>().SetText(Convert.ToString(dmg), isCrit);
    }
}
