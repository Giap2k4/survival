using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBaseController : MonoBehaviour, IUpdateManager
{
    protected ProjectileModel data;
    public SkillBaseController skillBaseController;

    protected virtual void OnEnable()
    {
        UpdateManager.instance.Register(this);
    }

    protected virtual void OnDisable()
    {
        if (UpdateManager.instance == null) return;
        UpdateManager.instance.UnRegister(this);
    }

    public virtual void InitData(ProjectileModel data, SkillBaseController skillBaseController)
    {
        this.data = data;
        this.skillBaseController = skillBaseController;
    }

    public void UpdateMe()
    {
        UpdateProjectile();
    }

    protected virtual void UpdateProjectile() { }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    /// <summary>
    /// Projectile chết
    /// </summary>
    protected virtual void Die() { }
}
