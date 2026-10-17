
using UnityEngine;
using UnityEngine.EventSystems;

public class DropManager : MonoBehaviour, IDropHandler
{
    public string correctDrop;
    public GameObject completedObject;

    private bool correct = false;
    public static int correctBoxes = 0;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        DragDrop item = eventData.pointerDrag.GetComponent<DragDrop>();

        if (item == null)
            return;

        RectTransform draggedRect = eventData.pointerDrag.GetComponent<RectTransform>();
        RectTransform dropRect = GetComponent<RectTransform>();

        if (item.item == correctDrop && !correct)
        {
            correct = true;
            correctBoxes++;

            // Snap to the drop zone's position
            draggedRect.position = dropRect.position;

            Debug.Log("Correct! " + correctBoxes);

            if (correctBoxes == 4)
            {
                completedObject.SetActive(true);
                Debug.Log("solved!");
            }
        }
        else
        {
            Debug.Log("Wrong!");
        }

        Debug.Log("OnDrop");
    }
}
