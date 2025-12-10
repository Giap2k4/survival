using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MoveSystemBase
{
    protected override void MoveAction()
    {
        transform.position = Vector3.MoveTowards(transform.position, BattleController.instance.GetPlayer().transform.position, moveSpeed * Time.deltaTime);
    }
}
