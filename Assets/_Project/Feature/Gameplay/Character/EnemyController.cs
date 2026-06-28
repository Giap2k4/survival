using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class EnemyController : CharacterBaseController
{
    protected int level;

    [SerializeField]
    protected float hp;

    [SerializeField] 
    protected float hpFirst;

    [SerializeField]
    protected float damage;

    protected AttackData attackData;
    [SerializeField]
    protected EnemyMove enemyMove;
    protected int typeEnemy;
    protected bool checkDmgAttack; // kiểm tra đã đủ thời gian gây dmg chưa
    [SerializeField]
    protected float damageInterval;
    protected float nextDamageTime = 0f;

    [SerializeField]
    protected GameObject parentHp;
    [SerializeField]
    protected SpriteRenderer HpScale;

    [SerializeField]
    protected float scaleXFirst;
    [SerializeField]
    protected float scaleYFirst;

    protected void Start()
    {
        scaleXFirst = 3.39f;
        scaleYFirst = 0.34f;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        // add modify stat nếu có level > 1
        //SetData();
        HpScale.size = new Vector2(3.39f, 0.34f); 
    }

    public override void SetLevel(int level) => this.level = level;

    protected override void SetData()
    {
        hp = stats.GetOrCreateStat(EnumBase.RPGStatType.Health).valueStat;
        hpFirst = hp;
        damage = stats.GetOrCreateStat(EnumBase.RPGStatType.Damage).valueStat;
    }

    public float GetDamageAttack() => damage;

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
        hp -= dmg;
        UpdateUI(dmg, isCrit);
        IsDead(dmg);
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
        
        if (hp <= 0)
        {
            parentHp.SetActive(false);
            enemyMove.SetIsDie();
            enemyMove.isDie = true;
            hp = 0;
            SpawnExp(); 
            PoolingManager.AddEnemyDisable(gameObject);
        }
    }

    protected void UpdateUI(float dmg, bool isCrit)
    {
        // bật thanh hp, hiện text HP bị trừ
        GameObject obj = DamageTextManager.instance.DamageText(transform.localPosition);

        obj.GetComponent<DamageTextController>().SetText(Convert.ToString(dmg), isCrit);

        parentHp.SetActive(true);
        float hpPercent = hp / hpFirst;

        float targetWidth;
        if (hpPercent <= 0) targetWidth = 0;
        else targetWidth = scaleXFirst * hpPercent;

        DOTween.To(() => HpScale.size.x, 
            x => HpScale.size = new Vector2(x, HpScale.size.y),
            targetWidth,
            0.2f).SetEase(Ease.OutQuad)
            .SetLink(gameObject);
    }
     
    public void OnDieAnimEnd()
    {
        
        gameObject.SetActive(false);
        // spawn exp ra vị trí đó luôn (có nhiều loại exp)
        

    }

    public void SetTypeEnemy(int type) => typeEnemy = type;

    /// <summary>
    /// Spawn gameObject exp
    /// </summary>
    protected void SpawnExp()
    {
        var obj = PoolingManager.GetExp("Exp" + typeEnemy);
        if (obj != null)
        {
            obj.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            obj.SetActive(true);
            return;
        }

        var prefab = Resources.Load<GameObject>("Exp" + typeEnemy);
        if (prefab == null) return;

        GameObject gObj = Instantiate(prefab, transform.position, Quaternion.identity);
        gObj.name = "Exp" + typeEnemy.ToString();
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Hero")) return;
        DamageInterval(collision);
    }

    public override void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Hero")) return;
        DamageInterval(collision);
    }
    
    protected void DamageInterval(Collision2D col)
    {
        if (Time.time < nextDamageTime) return;
        col.gameObject.GetComponent<PlayerController>().HandleDamageAttack(damage);
        nextDamageTime = Time.time + damageInterval;
    }
}
