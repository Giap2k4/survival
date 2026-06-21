 using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile5 : ProjectileBaseController
{
    [SerializeField]
    protected Collider2D col;

    [SerializeField]
    protected GameObject enemy;

    //protected GameObject objBoom;
    private Tween rotateTween;
    private bool checkDistance;

    protected override void OnEnable()
    {
        base.OnEnable();
        col.enabled = false;
        checkDistance = false;

        transform.DOKill();
        transform.rotation = Quaternion.identity;
        rotateTween = transform.DORotate(
                        new Vector3(0, 0, 360),
                        1f,
                        RotateMode.FastBeyond360)
                    .SetEase(Ease.Linear)
                    .SetLoops(-1);
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
        if (Vector3.Distance(transform.position, enemy.transform.position) < 0.1f && !checkDistance) HandleCollider();
    }

    protected void HandleCollider()
    {
        // bật component col lên
        col.enabled = true;
        checkDistance = true;
        // spawn hiệu ứng nổ
        GameObject obj = PoolingManager.GetBoomSkill5();
        obj.transform.SetPositionAndRotation(transform.position, transform.rotation);
        if (!obj.activeSelf) obj.SetActive(true);
    }
}
