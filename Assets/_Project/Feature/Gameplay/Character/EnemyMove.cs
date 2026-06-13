using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MoveSystemBase
{
    [SerializeField]
    protected Animator animator;

    [SerializeField]
    public GameObject hero;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] public Collider2D col;
    [SerializeField] protected bool isQuaterionLeft;
    public bool attacking;

    protected override void OnEnable()
    {
        base.OnEnable();
        col.enabled = true;
        animator.ResetTrigger("IsDead");
        //animator.Play("run");
        isDie = false;
    }

    protected override void Start()
    {
        base.Start();
        hero = BattleController.instance.GetPlayer();
    }

    protected override void MoveAction()
    {
        if (isDie) return;

        transform.position = Vector3.MoveTowards(transform.position, hero.transform.position, moveSpeed * Time.deltaTime);

        if (isQuaterionLeft)
        {
            if (hero.transform.position.x <= transform.position.x + 0.1f) spriteRenderer.flipX = false;
            else spriteRenderer.flipX = true;
            return;
        }

        if (hero.transform.position.x <= transform.position.x + 0.1f) spriteRenderer.flipX = true;
        else spriteRenderer.flipX = false;
    }

    public void SetIsDie()
    {
        // set collider nữa
        col.enabled = false;
        animator.SetTrigger("IsDead");
    }

    public virtual void Process() { }

    public virtual void AfterAttack() { }

}
