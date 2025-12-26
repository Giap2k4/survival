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
        foreach(var item in listSword)
        {
            item.SetActive(false);
            if (!listSwordPooling.Contains(item)) listSwordPooling.Add(item);
        }
        listSword.Clear();
        if (listSword.Count < data.projectileNumber)
        {
            var count = (data.projectileNumber - listSword.Count);
            for (int i = 0; i < count; i++)
            {
                GameObject obj = listSwordPooling.FirstOrDefault(x => x.activeInHierarchy == false);
                if (obj == null)
                {
                    obj = Instantiate(projectileSword, transform);
                    obj.GetComponent<ProjectileSword1>().SetAttackData(attackData);
                    listSword.Add(obj);
                    continue;
                }
                obj.GetComponent<ProjectileSword1>().SetAttackData(attackData);
                obj.SetActive(true);
                listSword.Add(obj);
            }
        }

        // tính số lượng rồi chia đều kiếm ra

        float rotate = 360f / listSword.Count;
        float a = 0f;

        Vector3 basePos = new Vector3(data.range.Value, 0f, 0f);

        for (int i = 0; i < listSword.Count; i++)
        {
            a = i * rotate;

            listSword[i].transform.localRotation = Quaternion.Euler(0f, 0f, a);
            listSword[i].transform.localPosition = Quaternion.Euler(0f, 0f, a) * basePos;
        }

    }
}
