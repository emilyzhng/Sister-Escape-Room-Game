using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// A knob on the wall that one medal can hang from.
// Shows the hand cursor when the mouse is over it.
public class MedalKnob : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public MedalDrag medal;

    private void Awake()
    {
        // invisible hover area so the mouse can "touch" the knob
        if (GetComponent<Image>() == null)
        {
            Image hoverArea = gameObject.AddComponent<Image>();
            hoverArea.color = new Color(1f, 1f, 1f, 0f);
        }
    }

    private void OnDisable()
    {
        HandCursor.Hide(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        HandCursor.Show(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HandCursor.Hide(this);
    }
}
