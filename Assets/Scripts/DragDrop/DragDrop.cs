using UnityEngine;
using UnityEngine.EventSystems;

public interface IDropSlot
{
    void PieceRemoved(DragDrop piece);
}

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    public string item;

    // go back to the start position if not dropped on the right spot
    public bool returnIfNotPlaced = false;

    [HideInInspector] public bool placed = false;
    [HideInInspector] public bool locked = false;
    [HideInInspector] public IDropSlot slot;
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
            eventData.pointerDrag = null;
            return;
        }

        Debug.Log("begin drag");
        placed = false;

        if (slot != null)
        {
            slot.PieceRemoved(this);
            slot = null;
        }
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
        canvasGroup.alpha = 0.5f;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (slot is IDropHandler slotDrop)
            slotDrop.OnDrop(eventData);
    }

    public void Lock()
    {
        locked = true;

        HandCursorOnHover hover = GetComponent<HandCursorOnHover>();
        if (hover != null)
            hover.enabled = false;
    }

    public Vector2 StartPosition => startPosition;

    public void SetStartPosition(Vector2 position)
    {
        startPosition = position;
        rectTransform.anchoredPosition = position;
    }

    public void ReturnToStart()
    {
        rectTransform.anchoredPosition = startPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("end drag");
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        if (returnIfNotPlaced && !placed)
            rectTransform.anchoredPosition = startPosition;
        else if (slot == null && !IsOnScreen())
            ReturnToStart();
    }

    bool IsOnScreen()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        Camera cam = canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay ? canvas.worldCamera : null;

        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        foreach (Vector3 corner in corners)
        {
            Vector2 point = RectTransformUtility.WorldToScreenPoint(cam, corner);
            if (point.x < 0 || point.y < 0 || point.x > Screen.width || point.y > Screen.height)
                return false;
        }

        return true;
    }
}
