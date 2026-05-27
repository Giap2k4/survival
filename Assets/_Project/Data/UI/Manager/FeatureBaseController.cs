using DG.Tweening;
using System;
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

    [SerializeField]
    protected EnumBase.UI screen;

    [SerializeField]
    protected RectTransform uiScale;

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

    public virtual void Show()
    {
        if (uiScale == null) return;
        else
        {
            if (screen == EnumBase.UI.Popup)
            {
                uiScale.transform.localScale = Vector3.zero;

                uiScale.transform.DOScale(Vector3.one, 0.3f)
                    .SetEase(Ease.OutBack)
                    .SetUpdate(true);
            }
        }
    }

    public virtual void Hide(Action onComplete = null)
    {
        if (screen == EnumBase.UI.Popup)
        {
            uiScale.transform.DOScale(Vector3.zero, 0.2f)
                .SetEase(Ease.InBack)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    onComplete?.Invoke();
                });
        }
        else if (screen == EnumBase.UI.FullScreen)
        {
            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();

            if (canvasGroup != null)
            {
                canvasGroup.DOFade(0, 0.2f)
                    .SetUpdate(true)
                    .OnComplete(() =>
                    {
                        onComplete?.Invoke();
                    }); 
            }
            else
            {
                Debug.LogWarning("Null");
                onComplete?.Invoke();
            }
        }
        else
        {
            onComplete?.Invoke();
        }
    }
}
