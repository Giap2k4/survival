using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupTriggerController : MonoBehaviour
{
    [SerializeField]
    protected PlayerController playerController;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Exp")
        {
            var component = collision.gameObject.GetComponent<ItemExpBattleController>();
            var type = component.GetTypeExp();
            var exp = DataManager.ExpBattle.GetExpBattleById(type);
            LevelExpController.instance.AddExp(exp.expNumber);

            // cho obj exp vào pooling
            //collision.gameObject.SetActive(false);
            //PoolingManager.AddExpPooling(collision.gameObject);
            component.CanMoveTargetHero();

        }
    }
}
