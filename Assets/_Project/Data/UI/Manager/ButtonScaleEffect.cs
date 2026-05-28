using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScaleEffect : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler,
    IPointerExitHandler
{
    [SerializeField] private Transform target;

    [SerializeField] private float pressScale = 0.9f;
    [SerializeField] private float duration = 0.1f;

    private Vector3 defaultScale;

    private void Awake()
    {
        if (target == null)
            target = transform;

        defaultScale = target.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        target.DOKill();

        target.DOScale(defaultScale * pressScale, duration)
            .SetEase(Ease.OutQuad)
            .SetUpdate(true)
            .SetLink(gameObject);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ResetScale();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ResetScale();
    }

    private void ResetScale()
    {
        target.DOKill();

        target.DOScale(defaultScale, duration)
            .SetEase(Ease.OutBack)
            .SetUpdate(true)
            .SetLink(gameObject);
    }

    private void OnDestroy()
    {
        target.DOKill();
    }
}