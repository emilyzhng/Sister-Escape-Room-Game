using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropBooks : MonoBehaviour, IDropHandler, IDropSlot
{
    public string correctDrop;
    public GameObject completedObject;

    private bool correct = false;
    public static int correctBoxes = 0;
    private DragDrop occupant;
    private static readonly List<DropBooks> allSlots = new List<DropBooks>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetSlots()
    {
        allSlots.Clear();
    }

    private void Awake()
    {
        allSlots.Add(this);
    }

    private void OnDestroy()
    {
        allSlots.Remove(this);
    }

    void LockAll()
    {
        foreach (DropBooks slot in allSlots)
        {
            if (slot.occupant != null)
                slot.occupant.Lock();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        DragDrop item = eventData.pointerDrag.GetComponent<DragDrop>();

        if (item == null)
            return;

        if (occupant != null && occupant != item)
        {
            item.ReturnToStart();
            return;
        }

        occupant = item;
        item.slot = this;
        item.placed = true;

        RectTransform draggedRect = eventData.pointerDrag.GetComponent<RectTransform>();
        RectTransform dropRect = GetComponent<RectTransform>();
        draggedRect.position = dropRect.position;

        if (item.item == correctDrop && !correct)
        {
            correct = true;
            correctBoxes++;

            Debug.Log("Correct! " + correctBoxes);

            if (correctBoxes == 4)
            {
                LockAll();
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

    public void PieceRemoved(DragDrop piece)
    {
        if (piece != occupant)
            return;

        occupant = null;

        if (!correct)
            return;

        correct = false;
        correctBoxes--;
        Debug.Log("Taken away! " + correctBoxes);
    }
}
