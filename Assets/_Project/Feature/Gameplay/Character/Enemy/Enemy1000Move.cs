using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy1000Move : EnemyMove
{
    protected Vector3 chargeDirection;
    protected float timeCount = 2;
    protected float speed = 0;
    [SerializeField]
    protected Enemy1000 controller;
    protected bool canAttack;

    protected override void OnEnable()
    {
        base.OnEnable();
        speed = 0;
        attacking = false;
    }

    public override void Process ()
    {
        chargeDirection = (hero.transform.position - transform.position).normalized;
    }
    protected override void MoveAction()
    {
        if (attacking)
        {
            transform.position += chargeDirection * moveSpeed * Time.deltaTime;
            return;
        }
        base.MoveAction();

        speed += Time.deltaTime;
        if (speed < timeCount) return;

        canAttack = true;
        float distance = Vector3.Distance(transform.position, hero.transform.position);

        if (canAttack && distance <= 3)
        {
            Process();
            controller.PlayRun0();
            attacking = true;
            col.isTrigger = true;
            speed = 0;
        }
    } 

    public override void AfterAttack()
    {
        attacking = false;
        col.isTrigger = false;
        speed = 0;
        canAttack = false;
    }
}
