using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile3 : ProjectileBaseController
{
    [SerializeField]
    protected GameObject sprite;

    protected Vector3 posFirst;
    protected bool checkGoBack;
    protected bool checkLimitRange;

    protected override void OnEnable()
    {
        base.OnEnable();
        posFirst = transform.position;
        checkGoBack = false;
        checkLimitRange = false;
    }

    protected override void ProjectileMove()
    {
        sprite.transform.Rotate(0f, 0f, 1000f * Time.deltaTime);
        //CheckViewPort();

        if (checkLimitRange) return;

        if (!checkGoBack)
        {
            CheckRange();
            transform.position += transform.right * GetProjectileSpeed() * Time.deltaTime;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, BattleController.instance.GetPlayer().transform.position, GetProjectileSpeed() * Time.deltaTime);
            CheckGoBackHero();
        }
    }

    protected void CheckRange()
    {

        float distance = Vector3.Distance(transform.position, posFirst);

        if (distance > GetRange())
        {
            checkLimitRange = true;
            RegisterCoroutine(WaitLimitRange());
        }
    }

    protected void CheckGoBackHero()
    {
        if (Vector3.Distance(transform.position, BattleController.instance.GetPlayer().transform.position) < 0.3f)
        {
            
            Die();
        }
    }

    IEnumerator WaitLimitRange()
    {
        // lấy từ SO
        yield return new WaitForSeconds(skillBaseController.HandleCustomValue1());
        //yield return new WaitForSeconds(0.1f);
        checkLimitRange = false;
        checkGoBack = true; 
    }
}
