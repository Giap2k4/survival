using DG.Tweening;
using UnityEngine;

public class ButtonShine : MonoBehaviour
{
    [SerializeField] private RectTransform shine;

    private Sequence sequence;

    private void Start()
    {
        PlayShine();
    }

    private void PlayShine()
    {
        if (shine == null) return;

        sequence?.Kill();

        shine.anchoredPosition = new Vector2(-500, 0);

        sequence = DOTween.Sequence();

        sequence.Append(
            shine.DOAnchorPosX(700, 1f)
                .SetEase(Ease.Linear)
        );

        sequence.AppendInterval(1.5f);

        sequence.SetLoops(-1, LoopType.Restart)
                .SetLink(gameObject);
    }

    private void OnDestroy()
    {
        sequence?.Kill();
    }
}