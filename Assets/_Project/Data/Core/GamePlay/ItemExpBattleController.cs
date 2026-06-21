using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemExpBattleController : MonoBehaviour, IUpdateManager
{
    [SerializeField]
    protected int typeExp;
    public float amplitude = 0.04f;   // độ cao
    public float frequency = 30f;     // tốc độ
    protected Vector3 startPos;
    protected bool checkUpdate;
    protected void OnEnable()
    {
        checkUpdate = false;
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
        if (checkUpdate)
        {
            transform.position = Vector3.MoveTowards(transform.position, BattleController.instance.GetPlayer().transform.position, 4 * Time.deltaTime);
            if (Vector3.Distance(transform.position, BattleController.instance.GetPlayer().transform.position) < 0.2f)
            {
                PoolingManager.AddExpPooling(gameObject);
                gameObject.SetActive(false);
            }

            return;
        }
        float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = startPos + Vector3.up * yOffset;
    }

    public void SetTypeExp(int type) => typeExp = type;
    public int GetTypeExp() => typeExp;

    public void CanMoveTargetHero()
    {
        checkUpdate = true;
    }
}
