using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile2 : ProjectileBaseController
{
    protected override void ProjectileMove()
    {
        transform.position += transform.right * Time.deltaTime * GetProjectileSpeed();
    }
}
