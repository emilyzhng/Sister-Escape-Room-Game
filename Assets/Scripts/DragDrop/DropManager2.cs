using UnityEngine;
using UnityEngine.EventSystems;

public class DropManagerboard : MonoBehaviour, IDropHandler
{
    public string correctDrop;
    public GameObject completedObject;

    private bool correct = false;
    public static int correctBoxes = 0;

    public void OnDrop(PointerEventData eventData)
    {
        DragDrop item = eventData.pointerDrag.GetComponent<DragDrop>();

        if (item.item == correctDrop && correct == false)
        {
            correct = true;
            correctBoxes++;
            Debug.Log("Correct! " + correctBoxes);

            if (correctBoxes == 6)
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

        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        }
    }
}

