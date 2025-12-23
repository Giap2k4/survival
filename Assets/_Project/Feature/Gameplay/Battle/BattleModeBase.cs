using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleModeBase : MonoBehaviour
{
    public int idMap = 0;
    protected int quantityEnemyMax;
    private float _outsideOffset = 0.08f; // Khoảng cách ngoài màn hình

    protected virtual void Start()
    {
        InitData();
    }
    protected virtual void OnEnable() { }
    protected virtual void OnDisable() { }

    protected abstract void InitData();

    protected virtual void InitHero()
    {
        int idHero = PlayerDataManager.Hero.database.idHeroSelected;
        var prefab = Resources.Load<GameObject>("_Prefab/Hero/Hero_" + idHero);
        GameObject obj = Instantiate(prefab);
        obj.transform.position = Vector3.zero;

        BattleController.instance.SetPlayer(obj);


        var hero = DataManager.Hero.GetHeroById(idHero);
        obj.GetComponent<CharacterBaseController>().AddStatBase(hero.powerStat, hero.level);

        // khởi tạo skill default
        obj.GetComponent<CharacterBaseController>().InitSkillDefault(hero.defaultSkill);
        BattleController.instance.cam.Follow = obj.transform;
    }

    protected virtual void SpawnEnemy()
    {
        var obj = DataManager.SpawnEnemy.GetSpawnEnemyById(idMap);
        int i = 0;
        
        foreach (var item in obj.details)
        {
            StartCoroutine(StartSpawnEnemy(item));
            i++;
        }
    }

    IEnumerator StartSpawnEnemy(SpawnEnemyDetails spawn)
    {
        yield return new WaitForSeconds(spawn.timeStart);

        Spawn(spawn.idEnemy, spawn.levelEnemy, spawn.spawnEnemyType, spawn.quantityEnemy);

        if (spawn.interval == 0) yield break;


        for (int i = 0; i < (spawn.timeEnd - spawn.timeStart) / spawn.interval; i++)
        {
            yield return new WaitForSeconds(spawn.interval);
            Spawn(spawn.idEnemy, spawn.levelEnemy, spawn.spawnEnemyType, spawn.quantityEnemy);
        }
    }

    protected virtual void Spawn(int idEnemy, int levelEnemy, EnumBase.SpawnEnemyType spawnType, int quantityEnemy)
    {
        for (int i = 0; i < quantityEnemy; i++)
        {
            Vector3 pos = GetRandomOutsidePosition(spawnType);

            GameObject obj = PoolingManager.GetEnemyDisable("Enemy_" + idEnemy);
            var enemy = DataManager.Enemy.GetEnemyById(idEnemy);

            // kiểm tra trong pool trước
            if (obj != null)
            {
                var script = obj.GetComponent<EnemyController>();
                script.AddStatBase(enemy.powerStat, levelEnemy);
                script.SetTypeEnemy(enemy.typeEnemy);

                obj.transform.SetPositionAndRotation(pos, Quaternion.identity);
                obj.SetActive(true);
                continue;
            }

            var prefab = Resources.Load<GameObject>("_Prefab/Enemy/Enemy_" + idEnemy);
            obj = Instantiate(prefab, pos, Quaternion.identity);
            
            var script1 = obj.GetComponent<EnemyController>();
            script1.AddStatBase(enemy.powerStat, levelEnemy);
            script1.SetTypeEnemy(enemy.typeEnemy);
            obj.name = "Enemy_" + idEnemy;

            PoolingManager.AddEnemyActive(obj);
        }
    }

    protected virtual Vector3 GetRandomOutsidePosition(EnumBase.SpawnEnemyType spawnType)
    {
        float vx = 0f, vy = 0f;

        switch (spawnType)
        {
            case EnumBase.SpawnEnemyType.Top:
                vy = 1f + _outsideOffset; // Quái xuất hiện ngoài phía trên màn hình
                vx = Random.Range(0f, 1f); // Random theo chiều ngang (trái, phải, giữa)
                break;

            case EnumBase.SpawnEnemyType.Bot:
                vy = -_outsideOffset; // Quái xuất hiện ngoài phía trên màn hình
                vx = Random.Range(0f, 1f); // Random theo chiều ngang (trái, phải, giữa)
                break;

            case EnumBase.SpawnEnemyType.Right:
                vx = 1f + _outsideOffset; // Quái xuất hiện ngoài phía trên màn hình
                vy = Random.Range(0f, 1f); // Random theo chiều ngang (trái, phải, giữa)
                break;

            case EnumBase.SpawnEnemyType.Left:
                vx = -_outsideOffset; // Quái xuất hiện ngoài phía trên màn hình
                vy = Random.Range(0f, 1f); // Random theo chiều ngang (trái, phải, giữa)
                break;
        }

        // Chuyển từ viewport sang world position
        Vector3 worldPos = BattleController.instance.mainCamera.ViewportToWorldPoint(new Vector3(vx, vy, BattleController.instance.mainCamera.nearClipPlane));
        worldPos.z = 0f;

        return worldPos;
    }
}
