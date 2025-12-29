using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile9 : Projectile8
{
    [SerializeField]
    protected SpriteRenderer spriteRenderer;

    public float pulseSpeed = 2f;     // tốc độ nháy
    public float minAlpha = 0.3f;
    public float maxAlpha = 0.6f;
    protected Vector3 scale;

    protected override void ResetData()
    {
        base.ResetData();
        transform.localScale = Vector3.zero;
        var a = GetProjectileSize();
        scale = new Vector3(a, a, a);

        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    protected override Vector2 GetDirection() => Vector2.zero;

    protected override void ProjectileMove()
    {
        transform.localScale = Vector3.MoveTowards(transform.localScale, scale, 0.5f * Time.deltaTime);

        float a = Mathf.Lerp(
            minAlpha,
            maxAlpha,
            Mathf.PingPong(Time.time * pulseSpeed, 1f)
        );

        Color c = spriteRenderer.color;
        c.a = a;
        spriteRenderer.color = c;
    }
}
