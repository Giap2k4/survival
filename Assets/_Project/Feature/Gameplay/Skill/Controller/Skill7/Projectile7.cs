using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile7 : ProjectileBaseController
{
    protected HashSet<Collider2D> collider2Ds = new HashSet<Collider2D>();
    protected AttackData dataProjectile;
    protected bool checkCloneData;

    protected override void ProjectileMove()
    {
        base.ProjectileMove();
        transform.Rotate(0, 0, 50 * Time.deltaTime);
    }

    protected override void ResetData()
    {
        base.ResetData();
        if (!checkCloneData)
        {
            dataProjectile = new AttackData(attackData);

            dataProjectile.stats[EnumBase.RPGStatType.Damage] *= skillBaseController.HandleCustomValue2();
            Debug.Log("Dmg: " + dataProjectile.stats[EnumBase.RPGStatType.Damage]);
            checkCloneData = true;
        }
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        // chạy coroutine
        if (collider2Ds.Add(collision))
        {
            RegisterCoroutine(DamageOverTime(collision));
        }
    }

    IEnumerator DamageOverTime(Collider2D col)
    {
        var wait = new WaitForSeconds(skillBaseController.HandleCustomValue1());

        while (collider2Ds.Contains(col) && col != null)
        {
            yield return wait;

            if (!collider2Ds.Contains(col) || col == null) yield break;

            if (!col.gameObject.activeSelf)
            {
                collider2Ds.Remove(col);
                yield break;
            }

            var enemy = col.gameObject.GetComponent<CharacterBaseController>();

            
            // truyền damage
            if (enemy != null)
            {
                enemy.TakeDamage(dataProjectile);
                HandleEffect();
            }
        }
    }

    protected void SetDmgOverTime()
    {

    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        collider2Ds.Remove(collision);
    }

    protected override void Die()
    {
        base.Die();
        collider2Ds.Clear();
    }
}
