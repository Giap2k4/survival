using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeatureBaseController : MonoBehaviour
{
    [Header("Cấu hình chung")]
    [SerializeField]
    protected EnumBase.Feature feature;

    [Header("Cấu hình chung")]
    [SerializeField]
    protected Button btnClose;

    protected virtual void OnDestroy()
    {
        foreach (var btn in GetComponentsInChildren<Button>())
            btn.onClick.RemoveAllListeners();

        StopAllCoroutines();
    }

    protected void CloseFeature()
    {
        UIManager.instance.CloseFeature(feature);
    }
}
