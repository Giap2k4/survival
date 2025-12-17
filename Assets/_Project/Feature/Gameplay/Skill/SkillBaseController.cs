using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillBaseController : MonoBehaviour
{
    [SerializeField]
    protected GameObject projectile;

    [SerializeField]
    protected int idSkill; // config trong inspecter

    [SerializeField]
    protected int levelCurrent = 1;

    [SerializeField]
    protected Transform parentProjectile;

    [SerializeField]
    protected List<GameObject> pooling = new List<GameObject>();

    protected SkillDetails skillDetails;

    protected virtual void Start()
    {
        string nameSkill = "Skill_" + idSkill;
        var skill = Resources.Load<SkillCollection>(nameSkill).dataGroups;
        StartCoroutine(StartSpawnProjectile(skill));
    }

    /// <summary>
    /// Khởi động spawn đạn
    /// </summary>
    /// <param name="skill"></param>
    /// <returns></returns>
    protected virtual IEnumerator StartSpawnProjectile(SkillModel skill)
    {
        skillDetails = skill.details.First(x => x.level == levelCurrent);
        Debug.Log(skillDetails.level);
        var cooldown = skillDetails.mechanicType.FirstOrDefault(x => x.mechanicTypes == EnumBase.MechanicTypes.Cooldown);

    Start:

        if (cooldown == null)
        {
            StartCoroutine(Spawn());
            yield return null;
        }
        else
        {
            float cooldownConvert = float.Parse(cooldown.values);
            if (cooldownConvert > 0)
            {
                yield return new WaitForSeconds(cooldownConvert);
                StartCoroutine(Spawn());
                goto Start;
            }
        }

    }

    /// <summary>
    /// Spawn projectile 
    /// </summary>
    /// <param name="skillDetails"></param>
    protected virtual IEnumerator Spawn()
    {
        var cooldown = GetCooldown();
        var duration = GetDuration();
        var damage = GetDamage();
        var range = GetRange();
        var detectRange = GetDetectRange();
        var projectileNumber = GetProjectileNumberModify();
        var projectileSpeed = GetProjectileSpeed();
        var projectileSize = GetProjectileSize();
        var targetFrom = GetTargetFrom();
        var targetTo = GetTargetTo();
        var rotate = GetDirection(targetTo, targetFrom);
        var fireRate = GetFireRate();
        parentProjectile = GetParentProjectile();

        ProjectileModel data = new ProjectileModel(cooldown, duration, damage, range, detectRange, projectileNumber, projectileSpeed, projectileSize, targetFrom, targetTo, fireRate);

        BeforeSpawnProjectile();

        if (projectileNumber > 0)
        {
            for (int i = 0; i < projectileNumber; i++)
            {
                if (fireRate != null) yield return new WaitForSeconds(1/fireRate.Value);
                SpawnProjectile(projectile, data, parentProjectile, rotate);
            }
        }

        AfterSpawnProjectile();

        yield return null;
    }

    /// <summary>
    /// Spawn 1 projectile
    /// </summary>
    protected virtual void SpawnProjectile(GameObject obj, ProjectileModel data, Transform parent, Quaternion rotate)
    {
        GameObject prefab = GetPooling();

        if (prefab != null)
        {
            prefab.transform.SetParent(parent == null ? null : parent, false);
            prefab.transform.SetLocalPositionAndRotation(data.targetFrom, rotate);
            InitDataProjectile(data, prefab);
            prefab.SetActive(true);

            RemoveObjPooling(prefab);
            return;
        }

        prefab = Instantiate(obj, data.targetFrom, rotate, parent == null ? null : parent);
        InitDataProjectile(data, prefab);
        prefab.name = "Projectile" + typeof(SkillBaseController).ToString();
    }

    /// <summary>
    /// Truyền data vào cho projectile
    /// </summary>
    /// <param name="data"></param>
    protected virtual void InitDataProjectile(ProjectileModel data, GameObject projectile)
    {
        var component = projectile.GetComponent<ProjectileBaseController>();
        component.InitData(data, this);
    }

    /// <summary>
    /// Lấy 1 phần tử trong pooling
    /// </summary>
    /// <returns></returns>
    protected virtual GameObject GetPooling()
    {
        if (pooling.Count > 0)
        {
            GameObject obj = pooling[0];
            return obj;
        }
        return null;
    }

    protected virtual void RemoveObjPooling(GameObject obj) => pooling.Remove(obj);

    /// <summary>
    /// Lấy giá trị cooldown từ config
    /// </summary>
    /// <param name="skillDetails"></param>
    /// <returns></returns>
    protected virtual float? GetCooldown()
    {
        var cooldown = skillDetails.mechanicType.FirstOrDefault(x => x.mechanicTypes == EnumBase.MechanicTypes.Cooldown);

        if (cooldown != null)
        {
            var value = float.Parse(cooldown.values);
            return value;
        }
        return null;
    }

    /// <summary>
    /// Lấy giá trị duration từ config
    /// </summary>
    /// <param name="skillDetails"></param>
    /// <returns></returns>
    protected virtual float? GetDuration()
    {
        var duration = skillDetails.mechanicType.FirstOrDefault(x => x.mechanicTypes == EnumBase.MechanicTypes.Duration);

        if (duration != null)
        {
            var value = float.Parse(duration.values);
            return value;
        }
        return null;
    }

    /// <summary>
    /// Lấy giá trị damage từ config
    /// </summary>
    /// <param name="skillDetails"></param>
    /// <returns></returns>
    protected virtual float[] GetDamage()
    {
        float[] damage = (skillDetails.mechanicType.FirstOrDefault(x => x.mechanicTypes == EnumBase.MechanicTypes.Damage).values).Split(',').Select(s => float.Parse(s.Trim())).ToArray();
        return damage;
    }

    /// <summary>
    /// Lấy giá trị range từ config
    /// </summary>
    /// <param name="skillDetails"></param>
    /// <returns></returns>
    protected virtual float? GetRange()
    {
        var range = skillDetails.mechanicType.FirstOrDefault(x => x.mechanicTypes == EnumBase.MechanicTypes.Range);

        if (range != null)
        {
            var value = float.Parse(range.values);
            return value;
        }
        return null;
    }

    /// <summary>
    /// Lấy giá trị detectRange từ config
    /// </summary>
    /// <param name="skillDetails"></param>
    /// <returns></returns>
    protected virtual float? GetDetectRange()
    {
        var detectRange = skillDetails.mechanicType.FirstOrDefault(x => x.mechanicTypes == EnumBase.MechanicTypes.DetectRang);

        if (detectRange != null)
        {
            var value = float.Parse(detectRange.values);
            return value;
        }
        return null;
    }

    /// <summary>
    /// Lấy giá trị ProjectileNumber từ config
    /// </summary>
    /// <param name="skillDetails"></param>
    /// <returns></returns>
    protected virtual int? GetProjectileNumber()
    {
        var ProjectileNumber = skillDetails.mechanicType.FirstOrDefault(x => x.mechanicTypes == EnumBase.MechanicTypes.ProjectileNumber);

        if (ProjectileNumber != null)
        {
            var value = int.Parse(ProjectileNumber.values);
            return value;
        }
        return null;
    }

    protected virtual int? GetProjectileNumberModify()
    {
        return GetProjectileNumber();
    }

    /// <summary>
    /// Lấy giá trị ProjectileSpeed từ config
    /// </summary>
    /// <param name="skillDetails"></param>
    /// <returns></returns>
    protected virtual float? GetProjectileSpeed()
    {
        var ProjectileSpeed = skillDetails.mechanicType.FirstOrDefault(x => x.mechanicTypes == EnumBase.MechanicTypes.ProjectileSpeed);

        if (ProjectileSpeed != null)
        {
            var value = float.Parse(ProjectileSpeed.values);
            return value;
        }
        return null;
    }

    /// <summary>
    /// Lấy giá trị projectileSize từ config
    /// </summary>
    /// <param name="skillDetails"></param>
    /// <returns></returns>
    protected virtual float? GetProjectileSize()
    {
        var projectileSize = skillDetails.mechanicType.FirstOrDefault(x => x.mechanicTypes == EnumBase.MechanicTypes.ProjectileSize);

        if (projectileSize != null)
        {
            var value = float.Parse(projectileSize.values);
            return value;
        }
        return null;
    }

    /// <summary>
    /// Lấy vị trí spawn projectile
    /// </summary>
    /// <param name="skillDetails"></param>
    /// <returns></returns>
    protected virtual Vector3 GetTargetFrom()
    {
        var targetFrom = skillDetails.mechanicType.First(x => x.mechanicTypes == EnumBase.MechanicTypes.TargetFrom);

        Vector3 posFrom = Vector3.zero;
        switch (Convert.ToInt32(targetFrom.values))
        {
            case 0:
                posFrom = GetPosHero();
                break;
        }

        return posFrom;
    } 

    /// <summary>
    /// Lấy vị trí điểm đến của projectile
    /// </summary>
    /// <param name="skillDetails"></param>
    /// <returns></returns>
    protected virtual Vector3 GetTargetTo()
    {
        var targetTo = skillDetails.mechanicType.First(x => x.mechanicTypes == EnumBase.MechanicTypes.TargetTo);

        Vector3 posTo = Vector3.zero;

        switch (Convert.ToInt32(targetTo.values))
        {
            case 0:
                posTo = GetPosHero();
                break;
        }

        return posTo;
    }

    /// <summary>
    /// Lấy giá trị fireRate từ config
    /// </summary>
    /// <param name="skillDetails"></param>
    /// <returns></returns>
    protected virtual float? GetFireRate()
    {
        var fireRate = skillDetails.mechanicType.FirstOrDefault(x => x.mechanicTypes == EnumBase.MechanicTypes.FireRate);

        if (fireRate != null)
        {
            var value = float.Parse(fireRate.values);
            return value;
        }
        return null;
    }

    /// <summary>
    /// Tính hướng bay của viên đạn
    /// </summary>
    /// <param name="posTo"></param>
    /// <param name="posFrom"></param>
    /// <returns></returns>
    protected virtual Quaternion GetDirection (Vector3 posTo, Vector3 posFrom)
    {
        return Quaternion.LookRotation((posTo - posFrom).normalized);
    }

    /// <summary>
    /// Lấy obj parent của projectile
    /// </summary>
    /// <returns></returns>
    protected virtual Transform GetParentProjectile()
    {
        if (parentProjectile == null) return null;
        return parentProjectile;
    }

    /// <summary>
    /// Làm gì đó sau khi spawn projectile
    /// </summary>
    protected virtual void AfterSpawnProjectile()
    {

    }

    /// <summary>
    /// Làm gì đó trước khi spawn projectile
    /// </summary>
    protected virtual void BeforeSpawnProjectile()
    {

    }

    /// <summary>
    /// Lấy vị trí của hero
    /// </summary>
    /// <returns></returns>
    protected Vector3 GetPosHero() => BattleController.instance.GetPlayer().transform.position;

    public void SetLevelSkill(int level) => levelCurrent = level;

    /// <summary>
    /// Làm gì đó với projectile khi nó vừa được sinh ra
    /// </summary>
    protected virtual void HandlerProjectile(ProjectileBaseController projectile)
    {

    }
}
