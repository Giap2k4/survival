using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile1 : ProjectileBaseController
{
    [SerializeField]
    protected List<GameObject> listSword = new List<GameObject>();

    [SerializeField]
    protected GameObject projectileSword;

    private bool checkInitData;

    protected override void OnEnable()
    {
        base.OnEnable();
        //if (checkInitData) SetRotateAndPositionSword();
    }

    protected override void UpdateProjectile()
    {
        transform.Rotate(0, 0, 90 * Time.deltaTime, Space.World);
    }

    public override void InitData(ProjectileModel data, SkillBaseController skillBaseController)
    {
        base.InitData(data, skillBaseController);
        SetRotateAndPositionSword();
        checkInitData = true;
    }

    protected void SetRotateAndPositionSword()
    {
        if (listSword.Count < data.projectileNumber)
        {
            var count = (data.projectileNumber - listSword.Count);
            for (int i = 0; i < count; i++)
            {
                GameObject obj = Instantiate(projectileSword, transform);
                listSword.Add(obj);
            }
        }

        // tính số lượng rồi chia đều kiếm ra
        var rotate = 360 / listSword.Count;
        listSword[0].transform.rotation = Quaternion.Euler(0, 0 , 0);

        Vector3 pos = new Vector3(data.range.Value, listSword[0].transform.position.y, listSword[0].transform.position.z);
        listSword[0].transform.localPosition = pos;

        for (int i = 1; i < listSword.Count; i++)
        {
            listSword[i].transform.rotation = Quaternion.Euler(0, 0, rotate);

            listSword[i].transform.localPosition = Quaternion.Euler(0, 0, rotate) * listSword[0].transform.localPosition;
            rotate += rotate;
        }
    }
}
