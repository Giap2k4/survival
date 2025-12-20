using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLastSurvivalMode : BattleModeBase
{
    public int idMap = 0;
    protected int quantityEnemyMax;
    private float _outsideOffset = 0.08f; // Khoảng cách ngoài màn hình

    protected override void InitData()
    {
        InitHero();
        SpawnEnemy();
    }

    protected void InitHero()
    {
        int idHero = PlayerDataManager.Hero.database.idHeroSelected;
        var prefab = Resources.Load<GameObject>("Hero_" + idHero);
        GameObject obj = Instantiate(prefab);
        obj.transform.position = Vector3.zero;

        BattleController.instance.SetPlayer(obj);

        
        var hero = DataManager.Hero.GetHeroById(idHero);
        obj.GetComponent<CharacterBaseController>().AddStatBase(hero.powerStat);

        // khởi tạo skill default
        obj.GetComponent<CharacterBaseController>().InitSkillDefault(hero.defaultSkill);

        obj.GetComponent<CharacterMovement>().joystick = BattleController.instance.joystick;
        BattleController.instance.cam.Follow = obj.transform;
    }

    protected void SpawnEnemy()
    {
        var obj = DataManager.SpawnEnemy.GetSpawnEnemyById(idMap);
        foreach (var item in obj.details)
        {
            StartCoroutine(StartSpawnEnemy(item));
        }
        
    }

    IEnumerator StartSpawnEnemy(SpawnEnemyDetails spawn)
    {
        yield return new WaitForSeconds(spawn.timeStart);
        Spawn(spawn.idEnemy, spawn.spawnEnemyType, spawn.quantityEnemy);

        if (spawn.interval == 0) yield return null;


        for (int i = 0; i < (spawn.timeEnd - spawn.timeStart)/spawn.interval; i++)
        {
            yield return new WaitForSeconds(spawn.interval);
            Spawn(spawn.idEnemy, spawn.spawnEnemyType, spawn.quantityEnemy);
        }
    }

    protected void Spawn(int idEnemy, EnumBase.SpawnEnemyType spawnType, int quantityEnemy)
    {
        for (int i = 0; i < quantityEnemy; i++)
        {
            Vector3 pos = GetRandomOutsidePosition(spawnType);

            // cho vào pool
            var prefab = Resources.Load<GameObject>("Enemy_" + idEnemy);
            GameObject obj = Instantiate(prefab, pos, Quaternion.identity);

            var enemy = DataManager.Enemy.GetEnemyById(idEnemy);
            obj.GetComponent<CharacterBaseController>().AddStatBase(enemy.powerStat);
            obj.name = "Enemy_" + idEnemy;

            // set level cho character 
        }
    }

    private Vector3 GetRandomOutsidePosition(EnumBase.SpawnEnemyType spawnType)
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
        worldPos.z = 0f; // Đảm bảo z=0 cho 2D game

        return worldPos;
    }

}
