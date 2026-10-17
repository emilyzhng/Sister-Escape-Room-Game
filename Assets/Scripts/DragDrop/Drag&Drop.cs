using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    public string item;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    } 
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("clicked");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("begin drag");
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
    }
}