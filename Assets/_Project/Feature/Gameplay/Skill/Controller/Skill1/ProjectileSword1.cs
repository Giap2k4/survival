using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSword1 : MonoBehaviour
{
    [SerializeField]
    protected AttackData attackData;

    public void SetAttackData(AttackData data) => attackData = data;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<CharacterBaseController>().TakeDamage(attackData);
        }
    }
}
