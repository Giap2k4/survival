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

    public bool isDie;

    private Vector3 scaleStart;
    private Vector3 scale;

    protected override void Start()
    {
        base.Start();
        hero = BattleController.instance.GetPlayer();
        scaleStart = transform.localScale;
        scale = scaleStart;
        scale.x *= -1;
    }

    protected override void MoveAction()
    {
        animator.SetBool("IsDie", isDie = false);

        transform.position = Vector3.MoveTowards(transform.position, hero.transform.position, moveSpeed * Time.deltaTime);

        if (hero.transform.position.x <= transform.position.x + 0.1f) transform.localScale = scale;
        else transform.localScale = scaleStart;
    }
}
