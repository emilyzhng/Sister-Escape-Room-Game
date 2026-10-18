using UnityEngine;
using UnityEngine.EventSystems;

public class HandCursorOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        HandCursor.Show(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HandCursor.Hide(this);
    }

    private void OnDisable()
    {
        HandCursor.Hide(this);
    }
}
