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
            var type = collision.gameObject.GetComponent<ItemExpBattleController>().GetTypeExp();
            var exp = DataManager.ExpBattle.GetExpBattleById(type);
            playerController.AddExp(exp.expNumber);

            // cho obj exp vào pooling
            collision.gameObject.SetActive(false);
            PoolingManager.AddExpPooling(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Enemy") return;
    }
}
