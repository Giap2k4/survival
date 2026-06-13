using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextManager : SingletonTemporary<DamageTextManager>
{
    [SerializeField]
    protected GameObject txt;
    public GameObject DamageText(Vector3 pos)
    {
        // lấy txt ở pool
        GameObject obj = PoolingManager.GetDamageText(txt);
        //obj.transform.SetParent(transform, false);
        obj.transform.position = pos;
        obj.GetComponent<DamageTextController>().Init(pos);
        obj.SetActive(true);
        return obj;
    }
}
