using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIDraggableImage : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public float speed = 1f; // Tốc độ di chuyển
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 minPosition;
    private Vector2 maxPosition;
    private Button button;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        button = GetComponent<Button>();

        Vector2 canvasSize = canvas.GetComponent<RectTransform>().sizeDelta;
        Vector2 imageSize = rectTransform.sizeDelta;

        minPosition.x = -canvasSize.x / 2 + imageSize.x / 2;
        maxPosition.x = canvasSize.x / 2 - imageSize.x / 2;
        minPosition.y = -canvasSize.y / 2 + imageSize.y / 2;
        maxPosition.y = canvasSize.y / 2 - imageSize.y / 2;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        button.interactable = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position = rectTransform.anchoredPosition;
        position.x += eventData.delta.x * speed * Time.deltaTime;
        position.y += eventData.delta.y * speed * Time.deltaTime;

        position.x = Mathf.Clamp(position.x, minPosition.x, maxPosition.x);
        position.y = Mathf.Clamp(position.y, minPosition.y, maxPosition.y);

        rectTransform.anchoredPosition = position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        button.interactable = false;
    }

    public void OnBackPressed()
    {
        Vector2 canvasCenter = Vector2.zero;
        rectTransform.anchoredPosition = canvasCenter;
        button.interactable = false;
    }
}
