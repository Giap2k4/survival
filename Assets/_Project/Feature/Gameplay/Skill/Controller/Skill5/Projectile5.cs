using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile5 : ProjectileBaseController
{
    [SerializeField]
    protected Collider2D col;

    [SerializeField]
    protected GameObject enemy;

    protected override void OnEnable()
    {
        base.OnEnable();
        col.enabled = false;
    }

    public override void InitData(ProjectileModel data, SkillBaseController skillBaseController)
    {
        base.InitData(data, skillBaseController);
        enemy = skillBaseController.GetNearest();
    }

    protected override void ProjectileMove()
    {
        if (enemy == null) return;
        transform.position = Vector3.MoveTowards(transform.position, enemy.transform.position, GetProjectileSpeed() * Time.deltaTime);
        if (Vector3.Distance(transform.position, enemy.transform.position) < 0.1f) HandleCollider();
    }

    protected void HandleCollider()
    {
        // bật component col lên
        col.enabled = true;
    }
}
