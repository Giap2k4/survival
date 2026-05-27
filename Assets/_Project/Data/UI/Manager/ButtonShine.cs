using DG.Tweening;
using UnityEngine;

public class ButtonShine : MonoBehaviour
{
    [SerializeField] private RectTransform shine;

    private void Start()
    {
        PlayShine();
    }

    private void PlayShine()
    {
        Sequence seq = DOTween.Sequence();

        seq.AppendCallback(() =>
        {
            shine.anchoredPosition = new Vector2(-500, 0);
        });

        seq.Append(
            shine.DOAnchorPosX(700, 1f)
                .SetEase(Ease.Linear)
        );

        seq.AppendInterval(1.5f);

        seq.SetLoops(-1);
    }
}