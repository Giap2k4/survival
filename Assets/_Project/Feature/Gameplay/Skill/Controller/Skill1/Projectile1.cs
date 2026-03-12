using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Projectile1 : ProjectileBaseController
{
    [SerializeField]
    protected List<GameObject> listSword = new List<GameObject>();
    protected List<GameObject> listSwordPooling = new List<GameObject>();

    [SerializeField]
    protected GameObject projectileSword;

    private bool checkInitData;

    //protected override void OnEnable()
    //{
    //    base.OnEnable();
    //    if (checkInitData) SetRotateAndPositionSword();
    //}

    protected override void UpdateProjectile()
    {
        transform.Rotate(0, 0, 90 * Time.deltaTime, Space.World);
    }

    public override void InitData(ProjectileModel data, SkillBaseController skillBaseController)
    {
        base.InitData(data, skillBaseController);
        SetRotateAndPositionSword();
        //checkInitData = true;
    }

    public void SetRotateAndPositionSword()
    {
        foreach (var item in listSword)
        {
            item.SetActive(false);
        }

        int currentCount = listSword.Count;
        int needed = data.projectileNumber.Value;

        for (int i = currentCount; i < needed; i++)
        {
            GameObject obj = Instantiate(projectileSword, transform);
            listSword.Add(obj);
        }

        for (int i = 0; i < needed; i++)
        {
            listSword[i].GetComponent<ProjectileSword1>().SetAttackData(attackData);
            listSword[i].SetActive(true);
        }

        float rotate = 360f / needed;
        float a = 0f;

        Vector3 basePos = new Vector3(data.range.Value, 0f, 0f);
        float size = data.projectileSize.Value;

        for (int i = 0; i < needed; i++)
        {
            a = i * rotate;

            listSword[i].transform.localRotation = Quaternion.Euler(0f, 0f, a);
            listSword[i].transform.localPosition = Quaternion.Euler(0f, 0f, a) * basePos;
            listSword[i].transform.localScale = new Vector3(size, size, size);
        }
    }
}
