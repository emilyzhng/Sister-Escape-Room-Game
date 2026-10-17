using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    public string item;

    // go back to the start position if not dropped on the right spot
    public bool returnIfNotPlaced = false;

    [HideInInspector] public bool placed = false;
    [HideInInspector] public bool locked = false;
    private Vector2 startPosition;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        startPosition = rectTransform.anchoredPosition;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("clicked");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (locked)
        {
            // already in its correct spot: cancel the drag
            eventData.pointerDrag = null;
            return;
        }

        Debug.Log("begin drag");
        placed = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
        canvasGroup.alpha = 0.5f;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("end drag");
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        if (returnIfNotPlaced && !placed)
            rectTransform.anchoredPosition = startPosition;
    }
}
