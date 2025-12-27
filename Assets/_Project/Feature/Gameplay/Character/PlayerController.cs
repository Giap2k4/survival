using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : CharacterBaseController
{
    [SerializeField]
    protected Animator animator;

    [SerializeField]
    protected Collider2D colPickup;

    [SerializeField]
    protected float hp;

    [SerializeField]
    protected Collider2D colliderHero;
    protected bool checkAttack;

    protected int defaultSkill; // id default skill

    protected override void SetData()
    {
        base.SetData();
        hp = stats.GetOrCreateStat(EnumBase.RPGStatType.Health).valueStat;
    }

    public void HandleDamageAttack(float dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            hp = 0;
            // anim die
            SetIsDie();
            EndGame();
        }
        // xử lý khi bị trừ HP (gọi hàm đến battleController xử lý)
    }

    public void SetIsDie()
    {
        colliderHero.enabled = false;
        colPickup.enabled = false;
        animator.SetTrigger("IsDead");
    }

    public override SkillBaseController InitSkillDefault(int idSkill)
    {
        var skill = SkillManager.instance.AddSkillDefault(base.InitSkillDefault(idSkill)); // add skill default vào list
        defaultSkill = skill.GetIdSkill();
        return skill;
    }

    public int GetIdDefaultSkill() => defaultSkill;
    protected void EndGame()
    {
        Time.timeScale = 0.5f;
        // tắt các collider, chờ 1s rồi spawn reward
        StartCoroutine(WaitEndGame());
    }

    IEnumerator WaitEndGame()
    {
        LevelExpController.instance.SetCanvasGroup(false);
        yield return new WaitForSeconds(1f);
        Time.timeScale = 0;

        // dọn dẹp enemy, skill, hero, projectile
        var obj = UIManager.instance.OpenFeature(EnumBase.Feature.Reward);
        obj.GetComponent<RewardController>().SetData(LevelExpController.instance.GetTimeSurvival());
        LevelExpController.instance.SetCanvasGroup(true);
    }
}
