using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile6 : ProjectileBaseController
{
    [SerializeField]
    protected Collider2D col;
    protected float rotate;
    protected Quaternion targetRotate;
    protected Quaternion rotateFirst;

    protected Vector3 scale;
    protected Vector3 scaleFirst;
    protected bool checkGoBack;
    protected bool checkLeft;

    public override void InitData(ProjectileModel data, SkillBaseController skillBaseController)
    {
        base.InitData(data, skillBaseController);
        checkLeft = false;
        Vector3 rotateLeft = transform.eulerAngles;
        if (BattleController.instance.joystick.HorizontalLast() < -0.01f)
        {
            checkLeft = true;
        }
        transform.rotation = Quaternion.Euler(rotateLeft);

        SetRotate(checkLeft);
        SetTargetRotate(checkLeft);
        scale = new Vector3(transform.localScale.x + 0.4f, transform.localScale.y + 0.4f, transform.localScale.z + 0.4f);
        scaleFirst = Vector3.one;
        rotateFirst = transform.rotation;
        col.enabled = false;
        checkGoBack = false;
    }
    protected override void ProjectileMove()
    {
        if (!checkGoBack)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, scale, 100 * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotate, 500 * Time.deltaTime);
            if (Quaternion.Angle(transform.rotation, targetRotate) < 0.1f)
            {
                RegisterCoroutine(WaitEff());
            }
            return;
        }

        transform.localScale = Vector3.MoveTowards(transform.localScale, scaleFirst, 100 * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateFirst, 500 * Time.deltaTime);

        if (Quaternion.Angle(transform.rotation, rotateFirst) < 0.1f)
        {
            col.enabled = true;
            RegisterCoroutine(WaitDie());
        }
    }

    IEnumerator WaitEff()
    {
        yield return new WaitForSeconds(0.2f);
        checkGoBack = true;
    }

    IEnumerator WaitDie()
    {
        yield return new WaitForSeconds(0.2f);
        Die();
    }

    protected void SetRotate(bool check)
    {
        rotate = skillBaseController.HandleCustomValue1();
        if (check) rotate *= -1;
    }

    protected void SetTargetRotate(bool check)
    {
        if (check)
        {
            targetRotate = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, (transform.eulerAngles.z + rotate));
            return;
        }
        targetRotate = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + rotate);
    }
}
