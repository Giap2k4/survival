using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UILightPulse : MonoBehaviour, IUpdateManager
{
    [Header("Pulse Settings")]
    public Color darkColor = new Color(1f, 0.75f, 0.2f, 0.6f);   // vàng tối
    public Color brightColor = new Color(1f, 0.9f, 0.4f, 1f);   // vàng sáng
    public float pulseSpeed = 2f; // tốc độ nháy

    [SerializeField]
    protected Image uiImage;

    void OnEnable()
    {
        UpdateManager.instance.Register(this);
    }

    void OnDisable()
    {
        if (UpdateManager.instance != null)
            UpdateManager.instance.UnRegister(this);
    }

    public void UpdateMe()
    {
        float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f;
        uiImage.color = Color.Lerp(darkColor, brightColor, t);
    }
}
