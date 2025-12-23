using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemExpBattleController : MonoBehaviour, IUpdateManager
{
    [SerializeField]
    protected int typeExp;
    public float amplitude = 0.02f;   // độ cao
    public float frequency = 10f;     // tốc độ
    protected Vector3 startPos;
    protected void OnEnable()
    {
        startPos = transform.position;
        UpdateManager.instance.Register(this);
    }

    protected void OnDisable()
    {
        if (UpdateManager.instance == null) return;
        UpdateManager.instance.UnRegister(this);
    }

    public void UpdateMe()
    {
        float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = startPos + Vector3.up * yOffset;
    }

    public void SetTypeExp(int type) => typeExp = type;
    public int GetTypeExp() => typeExp;
}
