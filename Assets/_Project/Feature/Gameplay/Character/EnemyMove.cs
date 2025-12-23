using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MoveSystemBase
{
    [SerializeField]
    protected Rigidbody2D rb;

    [SerializeField]
    protected Animator animator;

    [SerializeField]
    protected GameObject hero;

    [SerializeField] private SpriteRenderer spriteRenderer;

    protected override void Start()
    {
        base.Start();
        hero = BattleController.instance.GetPlayer();
        animator.SetBool("IsDie", false);
    }

    protected override void MoveAction()
    {
        if (isDie) return;

        transform.position = Vector3.MoveTowards(transform.position, hero.transform.position, moveSpeed * Time.deltaTime);

        if (hero.transform.position.x <= transform.position.x + 0.1f) spriteRenderer.flipX = true;
        else spriteRenderer.flipX = false;
    }

    public void SetIsDie()
    {
        // set collider nữa
        animator.SetTrigger("IsDead");
    }

    public void OnDieAnimEnd()
    {
        Debug.Log("Đã vào đây") ;
        // cho vào pooling
        gameObject.SetActive(false);

    }

}
