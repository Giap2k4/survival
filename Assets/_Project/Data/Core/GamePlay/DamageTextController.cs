using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTextController : MonoBehaviour, IUpdateManager
{
    [SerializeField]
    protected TextMeshPro txt;

    [SerializeField]
    protected Vector3 posY;
    protected Vector3 scale = new Vector3(1.5f, 1.5f, 1.5f);
    protected bool checkTime;

    protected void OnDisable()
    {
        if (UpdateManager.instance == null) return;
        UpdateManager.instance.UnRegister(this);
    }

    private void OnEnable()
    {
        UpdateManager.instance.Register(this);
        txt.color = Color.white;
        txt.fontSize = 2;

        checkTime = false;
    }

    public void Init(Vector3 pos)
    {
        posY = pos + new Vector3(0, 0.4f, 0);
    }

    public void SetText(string text, bool isCrit = false)
    {
        txt.text = text;
        if (isCrit)
        {
            txt.color = Color.red;
            txt.fontSize = 3.5f;
        }
    }

    public void UpdateMe()
    {
        transform.position = Vector3.MoveTowards(transform.position, posY, 2 * Time.deltaTime);
        transform.localScale = Vector3.MoveTowards(transform.localScale, scale, 5 * Time.deltaTime);

        float distance = Vector3.Distance(transform.position, posY);

        if (distance <= 0.1f)
        {
            if (!checkTime) StartCoroutine(WaitText());
        }
    }

    IEnumerator WaitText()
    {
        checkTime = true;
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
        PoolingManager.AddDamageTextPool(gameObject);
    }
}
