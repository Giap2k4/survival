using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveSystemBase : MonoBehaviour, IUpdateManager
{
    public float moveSpeed = 0;
    protected abstract void MoveAction(); // xử lý di chuyển
    public void SetSpeed(float speed) => moveSpeed = speed;

    protected virtual void Start()
    {
        moveSpeed = gameObject.GetComponent<CharacterBaseController>().stats.GetOrCreateStat(EnumBase.RPGStatType.MoveSpeed).valueStat;
    }

    protected virtual void OnEnable()
    {
        UpdateManager.instance.Register(this);
    }

    protected virtual void OnDisable()
    {
        if (UpdateManager.instance == null) return;
        UpdateManager.instance.UnRegister(this);
    }

    public virtual void UpdateMe()
    {
        MoveAction();
    }
}
