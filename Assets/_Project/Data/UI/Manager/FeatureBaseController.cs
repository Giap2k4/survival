using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeatureBaseController : MonoBehaviour
{
    [Header("Cấu hình chung")]
    [SerializeField]
    protected EnumBase.Feature feature;

    [SerializeField]
    protected Button btnClose;

    protected virtual void OnDestroy()
    {
        foreach (var btn in GetComponentsInChildren<Button>())
            btn.onClick.RemoveAllListeners();

        StopAllCoroutines();
    }

    protected virtual void Awake() { }

    protected virtual void Start()
    {
        if (btnClose != null) btnClose.onClick.AddListener(CloseFeature);
        LoadData();
    }

    protected virtual void OnEnable() { }
    protected virtual void OnDisable() { }

    protected void CloseFeature()
    {
        UIManager.instance.CloseFeature(feature);
    }

    protected virtual void LoadData() { }
}
