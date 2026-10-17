using UnityEngine;
using UnityEngine.EventSystems;

public class DropManagerstand : MonoBehaviour, IDropHandler
{
    public string correctDrop;
    public GameObject completedObject;
    public bool need;

    private bool correct = false;
    public static int correctBoxes = 0;

    public void OnDrop(PointerEventData eventData)
    {
        DragDrop item = eventData.pointerDrag.GetComponent<DragDrop>();

        if (item.item == correctDrop && correct == false)
        {
            correct = true;

            if (need)
            {
                correctBoxes++;
                Debug.Log("Correct! " + correctBoxes);

                if (correctBoxes == 9)
                {
                    completedObject.SetActive(true);
                    Debug.Log("solved!");
                }
            }
            else
            {
                Debug.Log("Correct, but not needed!");
            }
        }
        else
        {
            Debug.Log("Wrong!");
        }

        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().position =
    GetComponent<RectTransform>().position;
        }
    }
}