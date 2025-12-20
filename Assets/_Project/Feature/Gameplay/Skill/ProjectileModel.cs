using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileModel
{
    public float? cooldown;
    public float? duration;
    public float[] damage;
    public float? range;
    public float? detectRange;
    public int? projectileNumber;
    public float? projectileSpeed;
    public float? projectileSize;
    public Vector3 targetFrom;
    public Vector3 targetTo;
    public float? fireRate;
    public Vector2 direction;

    public ProjectileModel(
        float? cooldown,
        float? duration,
        float[] damage,
        float? range,
        float? detectRange,
        int? projectileNumber,
        float? projectileSpeed,
        float? projectileSize,
        Vector3 targetFrom,
        Vector3 targetTo,
        float? fireRate,
        Vector2 direction
    )
    {
        this.cooldown = cooldown;
        this.duration = duration;
        this.damage = damage;
        this.range = range;
        this.detectRange = detectRange;
        this.projectileNumber = projectileNumber;
        this.projectileSpeed = projectileSpeed;
        this.projectileSize = projectileSize;
        this.targetFrom = targetFrom;
        this.targetTo = targetTo;
        this.fireRate = fireRate;
        this.direction = direction;
    }
}
