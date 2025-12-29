using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextManager : SingletonTemporary<DamageTextManager>
{
    [SerializeField]
    protected GameObject txt;
    public GameObject DamageText(Vector3 pos)
    {
        return Instantiate(txt, pos, Quaternion.identity, transform);
    }
}
