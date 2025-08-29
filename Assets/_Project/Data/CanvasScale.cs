using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class CanvasScalerMatch : MonoBehaviour
{
    [SerializeField]
    protected CanvasScaler scaler;

    void Start()
    {
        UpdateMatch();
    }

    void UpdateMatch()
    {
        float aspect = (float)Screen.height / Screen.width;

        // Ví dụ: nếu aspect > 1.6 (màn hình dài, như iPhone 12) → match = 0
        // Nếu aspect <= 1.6 (màn hình ngắn, như iPad) → match = 1
        if (aspect > 1.6f)
        {
            scaler.matchWidthOrHeight = 0f;  // Ưu tiên chiều ngang
        }
        else
        {
            scaler.matchWidthOrHeight = 1f;  // Ưu tiên chiều dọc
        }
    }
}
