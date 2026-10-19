using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropMusicNotes : MonoBehaviour, IDropHandler, IDropSlot
{
    public string correctDrop;
    public GameObject completedObject;
    public bool need;
    public string hintItem = "MusicSheet";

    private bool correct = false;
    public static int correctBoxes = 0;
    private DragDrop occupant;
    private static readonly List<DropMusicNotes> allSlots = new List<DropMusicNotes>();

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
        foreach (DropMusicNotes slot in allSlots)
        {
            if (slot.occupant != null)
                slot.occupant.Lock();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        DragDrop item = eventData.pointerDrag.GetComponent<DragDrop>();

        if (occupant != null && occupant != item)
        {
            item.ReturnToStart();
            return;
        }

        occupant = item;
        item.slot = this;

        if (item.item == correctDrop && correct == false)
        {
            correct = true;

            if (need)
            {
                correctBoxes++;
                Debug.Log("Correct! " + correctBoxes);

                if (correctBoxes == 9)
                {
                    LockAll();
                    completedObject.SetActive(true);

                    Inventory inventory = FindFirstObjectByType<Inventory>();
                    if (inventory != null)
                        inventory.RemoveItem(hintItem);

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

    public void PieceRemoved(DragDrop piece)
    {
        if (piece != occupant)
            return;

        occupant = null;

        if (!correct)
            return;

        correct = false;

        if (need)
        {
            correctBoxes--;
            Debug.Log("Taken away! " + correctBoxes);
        }
    }
}
