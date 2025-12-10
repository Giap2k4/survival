using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleModeBase : MonoBehaviour
{
    protected virtual void Start()
    {
        InitData();
    }
    protected virtual void OnEnable() { }
    protected virtual void OnDisable() { }

    protected abstract void InitData();
}
