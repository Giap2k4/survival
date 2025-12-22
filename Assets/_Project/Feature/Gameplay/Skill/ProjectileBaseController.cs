using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileBaseController : MonoBehaviour, IUpdateManager
{
    protected ProjectileModel data;
    public SkillBaseController skillBaseController;
    protected AttackData attackData;
    protected bool checkDuration;

    protected bool checkIsPierce;
    protected float numberPierce;
    protected float countPierce;
    protected bool isPierceAllEnemy;

    protected List<IEnumerator> listCoroutine = new List<IEnumerator>();

    protected virtual void Awake() { }

    protected virtual void OnEnable()
    {
        UpdateManager.instance.Register(this);
        checkDuration = false;
        //ResetData();
    }

    protected virtual void OnDisable()
    {
        // hủy các coroutine
        foreach (var item in listCoroutine)
        {
            StopCoroutine(item);
        }

        if (UpdateManager.instance == null) return;
        UpdateManager.instance.UnRegister(this);
    }

    protected virtual void RegisterCoroutine(IEnumerator callback)
    {
        StartCoroutine(callback);
        listCoroutine.Add(callback);
    }

    public virtual void InitData(ProjectileModel data, SkillBaseController skillBaseController)
    {
        this.data = data;
        this.skillBaseController = skillBaseController;

        ResetData();
    }

    public void UpdateMe()
    {
        UpdateProjectile();
    }


    protected virtual void ResetData ()
    {
        
    }

    protected virtual void UpdateProjectile() 
    {
        CheckViewPort();
        if (!checkDuration) StartCoroutine(HandleDuration());
        ProjectileMove();
    }

    protected virtual void ProjectileMove()
    {
        transform.position += (Vector3)GetDirection() * GetProjectileSpeed() * Time.deltaTime;
    }

    IEnumerator HandleDuration()
    {
        checkDuration = true;
        if (GetDuration() == 0) yield break;
        yield return new WaitForSeconds(GetDuration());
        // cho vào pool
        Die();
    }

    protected virtual void CheckViewPort()
    {
        Vector3 worldPos = transform.position;
        Vector3 vp = BattleController.instance.mainCamera.WorldToViewportPoint(worldPos);

        float vx = vp.x;
        float vy = vp.y;

        float margin = 0.5f;

        if (vx > 1f + margin || vx < -margin ||
            vy > 1f + margin || vy < -margin) Die();
    }

    protected virtual Vector3 GetTargetFrom() => data.targetFrom;
    protected virtual Vector3 GetTargetTo() => data.targetTo;

    protected virtual float GetDuration() => data.duration.GetValueOrDefault(0);
    protected virtual float GetProjectileSize() => data.projectileSize.Value;

    protected virtual float GetProjectileSpeed() => data.projectileSpeed.Value;

    protected virtual Vector2 GetDirection() => data.direction;
    protected virtual float GetRange() => data.range.GetValueOrDefault(0);

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<CharacterBaseController>().TakeDamage(attackData);

            // xử lý nếu có effect (đa phần chỉ cần add eff vào attackData rồi enemy tự xử lý)
            HandleEffect();

            if (isPierceAllEnemy) return;
            countPierce++;
            if (countPierce >= numberPierce) Die();
        }

    }

    protected virtual void HandleEffect()
    {
        if (attackData.effects.Count > 0)
        {
            foreach (var item in attackData.effects)
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
    }

    protected virtual void OnTriggerStay2D(Collider2D collision) { }
    protected virtual void OnTriggerExit2D(Collider2D collision) { }

    /// <summary>
    /// Projectile chết
    /// </summary>
    protected virtual void Die() 
    {
        // cho vào pool
        gameObject.SetActive(false);
        PoolingManager.AddProjectilePooling(gameObject);
    }

    public virtual void SetAttackData(AttackData data) => attackData = data;

    /// <summary>
    /// xử lý eff đẩy lùi
    /// </summary>
    protected virtual void HandleEffectKnockBack () { }

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
    protected virtual void HandleEffectPierce() 
    {
        checkIsPierce = true;

        var value = attackData.effects[EnumBase.EffectType.Pierce];

        if (value[1] == -1) isPierceAllEnemy = true;
        else numberPierce = value[1];
    }

    /// <summary>
    /// Xử lý eff làm chậm
    /// </summary>
    protected virtual void HandleEffectSlow() { }

    /// <summary>
    /// xử lý eff choáng
    /// </summary>
    protected virtual void HandleEffectStun() { }
}
