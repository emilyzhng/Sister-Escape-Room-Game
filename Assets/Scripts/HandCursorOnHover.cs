using UnityEngine;
using UnityEngine.EventSystems;

public class HandCursorOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEndDragHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        HandCursor.Show(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!eventData.dragging)
            HandCursor.Hide(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        HandCursor.Hide(this);
    }

    private void OnDisable()
    {
        HandCursor.Hide(this);
    }
}
