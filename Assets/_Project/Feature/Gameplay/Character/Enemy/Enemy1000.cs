using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1000 : EnemyController
{
    public Animator anim;

    protected override void OnEnable()
    {
        base.OnEnable();
        enemyMove.moveSpeed = enemyMove.moveSpeedFirst;
    }

    public void PlayRun0()
    {
        enemyMove.moveSpeed = 0;
        anim.Play("run0");
       
    }

    public void PlayRun1()
    {
        damage *= 5;
        enemyMove.moveSpeed = enemyMove.moveSpeedFirst * 2.5f;
        anim.Play("run1");
    }

    public void PlayAttack()
    {
        enemyMove.moveSpeed /= 2;  
        anim.Play("attack");
    }

    public void PlayWalk()
    {
        damage /= 5;
        enemyMove.AfterAttack();
        anim.Play("walk");
    }

    public void BossDie() => enemyMove.moveSpeed = 0;

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (!collision.CompareTag("Hero")) return;

        collision.GetComponent<PlayerController>()
             .HandleDamageAttack(damage); 
    }
}
