using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField]
    private RectTransform background; // cha

    [SerializeField]
    private RectTransform handle; // con
    private Vector2 inputVector;

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background, eventData.position, eventData.pressEventCamera, out pos);

        pos.x = (pos.x / background.sizeDelta.x) * 2;
        pos.y = (pos.y / background.sizeDelta.y) * 2;

        inputVector = new Vector2(pos.x, pos.y);
        inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

        // di chuyển handle
        handle.anchoredPosition = new Vector2(
            inputVector.x * (background.sizeDelta.x / 2),
            inputVector.y * (background.sizeDelta.y / 2));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }

    public float Horizontal() { return inputVector.x; }
    public float Vertical() { return inputVector.y; }
    public Vector2 Direction() { return inputVector; }
}
