using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile8 : ProjectileBaseController
{
    protected HashSet<Collider2D> collider2Ds = new HashSet<Collider2D>();

    protected override void ProjectileMove()
    {
        transform.Rotate(0, 0, 50 * Time.deltaTime);
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
        var enemy = col.gameObject.GetComponent<CharacterBaseController>();
        // truyền damage
        if (enemy != null)
        {
            enemy.TakeDamage(attackData);
            HandleEffect();
        }

        while (collider2Ds.Contains(col) && col != null)
        {
            yield return wait;

            if (!collider2Ds.Contains(col) || col == null) yield break;

            if (!col.gameObject.activeSelf)
            {
                collider2Ds.Remove(col);
                yield break;
            }

            // truyền damage
            if (enemy != null)
            {
                enemy.TakeDamage(attackData);
                HandleEffect();
            }
        }
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
