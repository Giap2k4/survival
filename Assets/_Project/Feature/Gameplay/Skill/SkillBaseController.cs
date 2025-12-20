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

    protected SkillModel skillModel;
    protected SkillDetails skillDetails;
    protected AttackData attackData = new AttackData();
    protected CharacterBaseController characterBaseController;
    public Dictionary<EnumBase.EffectType, float[]> effect = new Dictionary<EnumBase.EffectType, float[]>();
    protected virtual void Start()
    {
        string nameSkill = "Skill_" + idSkill; 
        skillModel = Resources.Load<SkillCollection>(nameSkill).dataGroups;
        characterBaseController = BattleController.instance.GetPlayer().GetComponent<CharacterBaseController>();
        
        StartCoroutine(StartSpawnProjectile(skillModel));
    }

    /// <summary>
    /// Khởi động spawn đạn
    /// </summary>
    /// <param name="skill"></param>
    /// <returns></returns>
    protected virtual IEnumerator StartSpawnProjectile(SkillModel skill)
    {
        skillDetails = skill.details.First(x => x.level == levelCurrent);
        var cooldown = skillDetails.mechanicType.FirstOrDefault(x => x.mechanicTypes == EnumBase.MechanicTypes.Cooldown);
        AddEffect();
        InitAttackData();
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
        ProjectileModel data = InitProjectileData();

        BeforeSpawnProjectile();

        if (GetProjectileNumberModify() > 0)
        {
            for (int i = 0; i < GetProjectileNumberModify(); i++)
            {
                if (GetFireRate() != null) yield return new WaitForSeconds(1/ GetFireRate().Value);
                SpawnProjectile(projectile, data, parentProjectile, GetRotate(GetTargetTo(), GetTargetFrom()));
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

            return;
        }

        prefab = Instantiate(obj, data.targetFrom, rotate, parent == null ? null : parent);
        // truyền AttackData vào cho projectile
        prefab.GetComponent<ProjectileBaseController>().SetAttackData(attackData);
        InitDataProjectile(data, prefab);
        prefab.name = "Projectile" + this.GetType().Name;
        AfterSpawn1Projectile();
    }

    /// <summary>
    /// Làm gì đó sau khi spawn 1 projectile
    /// </summary>
    protected virtual void AfterSpawn1Projectile() { }

    protected virtual void InitAttackData()
    {
        // add các giá trị stat cần (nhân với chỉ số đã config trong csv)
        attackData.stats.Add(EnumBase.RPGStatType.Damage, GetValueStat(EnumBase.RPGStatType.Damage));
        attackData.stats.Add(EnumBase.RPGStatType.CritRate, GetValueStat(EnumBase.RPGStatType.CritRate));
        attackData.stats.Add(EnumBase.RPGStatType.CritDamage, GetValueStat(EnumBase.RPGStatType.CritDamage));

        // add các eff từ skill
        foreach (var item in effect)
        {
            // Add eff
            attackData.effects.Add((EnumBase.EffectType)item.Value[0], item.Value);
        }
    }

    protected virtual ProjectileModel InitProjectileData()
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
        var direction = GetDirection(targetTo, targetFrom);
        var fireRate = GetFireRate();
        parentProjectile = GetParentProjectile();

        return new ProjectileModel(cooldown, duration, damage, range, detectRange, projectileNumber, projectileSpeed, projectileSize, targetFrom, targetTo, fireRate, (Vector2)direction);
    }

    protected void AddEffect()
    {
        foreach (var item in skillDetails.mechanicType)
        {
            if (item.mechanicTypes.ToString().StartsWith("effect_"))
            {
                float[] values = item.values
                            .Split(',')
                            .Select(s => float.Parse(s.Trim()))
                            .ToArray();

                effect.Add((EnumBase.EffectType)values[0], values);
            }
        }
    }

    protected float GetValueStat(EnumBase.RPGStatType type)
    {
        // nhân với các chỉ số trong csv nữa
        return characterBaseController.stats.GetOrCreateStat(type).valueStat;
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
        return PoolingManager.GetProjectilePooling("Projectile" + this.GetType().Name);
    }

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
        var detectRange = skillDetails.mechanicType.FirstOrDefault(x => x.mechanicTypes == EnumBase.MechanicTypes.DetectRange);

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
            case 1:
                posFrom = GetNearest();
                break;

            case 2:
                posFrom = GetRandom();
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

            case 1:
                posTo = GetNearest();
                break;

            case 2:
                posTo = GetRandom();
                break;

            case 3:
                posTo = GetJoyStick();
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
    /// Hướng bay
    /// </summary>
    /// <param name="posTo"></param>
    /// <param name="posFrom"></param>
    /// <returns></returns>
    protected virtual Vector2 GetDirection (Vector3 posTo, Vector3 posFrom)
    {
        Vector2 dir;

        var targetTo = skillDetails.mechanicType.First(x => x.mechanicTypes == EnumBase.MechanicTypes.TargetTo);
        if (Convert.ToInt32(targetTo.values) == 3)
        {
            dir = BattleController.instance.joystick.Direction();
            return dir;
        }

        dir = ((Vector2)posTo - (Vector2)posFrom).normalized;

        return dir;
    }

    protected virtual Quaternion GetRotate(Vector3 posTo, Vector3 posFrom)
    {
        Vector2 dir = GetDirection(posTo, posFrom);

        if (dir.sqrMagnitude < 0.0001f)
            return Quaternion.identity;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        return Quaternion.Euler(0f, 0f, angle);
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

    /// <summary>
    /// Lấy vị trí quái gần nhất, nếu k có trả về vị trí hero
    /// </summary>
    /// <returns></returns>
    protected Vector3 GetNearest()
    {
        Vector3 posNearest = Vector3.zero;
        var listObjEnemy = DetectInCircle(GetPosHero(), GetDetectRange().Value);

        Vector3 posHero = GetPosHero();
        if (listObjEnemy.Count == 0) return posHero;
        float distance = 0;
        float temp = 0;

        for (int i = 0; i < listObjEnemy.Count; i++)
        {
            if (i == 0)
            {
                distance = Vector3.Distance(posHero, listObjEnemy[i].transform.position);
                posNearest = listObjEnemy[i].transform.position;
                continue;
            }

            temp = Vector3.Distance(posHero, listObjEnemy[i].transform.position);

            if (distance > temp)
            {
                posNearest = listObjEnemy[i].transform.position;
                distance = temp;
            }
        }

        return posNearest;
    }

    /// <summary>
    /// Lấy random 1 trong số các quái detect được
    /// </summary>
    /// <returns></returns>
    public Vector3 GetRandom()
    {
        var listObjEnemy = DetectInCircle(GetPosHero(), GetDetectRange().Value);

        var random = UnityEngine.Random.Range(0, listObjEnemy.Count + 1);

        return listObjEnemy[random].transform.position;
    }

    public Vector3 GetJoyStick()
    {
        return (Vector3)BattleController.instance.joystick.Direction();
    }

    public static List<GameObject> DetectInCircle(Vector3 center, float detectRange)
    {
        var col = Physics2D.OverlapCircleAll(center, detectRange);
        List<GameObject> list = new List<GameObject>();

        foreach (var item in col)
        {
            if (item.gameObject.tag == "Enemy")
            {
                list.Add(item.gameObject);
            }
        }
        return list;
    }

    public void SetLevelSkill(int level) => levelCurrent = level;

    /// <summary>
    /// Làm gì đó với projectile khi nó vừa được sinh ra
    /// </summary>
    protected virtual void HandlerProjectile(ProjectileBaseController projectile)
    {

    }

    public virtual void LevelUp()
    {
        SetLevelSkill(levelCurrent + 1);
        // khởi động lại spawn , Set lại AttackData
    }

    public virtual float HandleCustomValue1()
    {
        return float.Parse( skillDetails.mechanicType.FirstOrDefault(x => x.mechanicTypes == EnumBase.MechanicTypes.custom_value_1).values);
        
    }

    public virtual float HandleCustomValue2()
    {
        return float.Parse(skillDetails.mechanicType.FirstOrDefault(x => x.mechanicTypes == EnumBase.MechanicTypes.custom_value_2).values);

    }
}
