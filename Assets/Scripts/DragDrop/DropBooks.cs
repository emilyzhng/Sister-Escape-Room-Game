using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropBooks : MonoBehaviour, IDropHandler
{
    public string correctDrop;
    public GameObject completedObject;

    private static readonly List<DropBooks> allSlots = new List<DropBooks>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void resetBetweenPlays()
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

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        DragDrop item = eventData.pointerDrag.GetComponent<DragDrop>();

        if (item == null)
            return;

        foreach (DragDrop other in FindObjectsByType<DragDrop>(FindObjectsSortMode.None))
        {
            if (other != item && isInSlot(other))
                other.ReturnToStart();
        }

        eventData.pointerDrag.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;
        item.placed = true;
        Debug.Log(item.item == correctDrop ? "Correct!" : "Wrong!");

        checkSolved();
    }

    bool isInSlot(DragDrop piece)
    {
        return Vector2.Distance(piece.transform.position, transform.position) < 1f;
    }

    void checkSolved()
    {
        DragDrop[] pieces = FindObjectsByType<DragDrop>(FindObjectsSortMode.None);
        List<DragDrop> inPlace = new List<DragDrop>();

        foreach (DropBooks slot in allSlots)
        {
            DragDrop piece = slot.pieceInSlot(pieces);

            if (piece != null)
                inPlace.Add(piece);
        }

        Debug.Log("In place: " + inPlace.Count + " / " + allSlots.Count);

        if (inPlace.Count < allSlots.Count)
            return;

        foreach (DragDrop piece in inPlace)
            piece.locked = true;

        if (completedObject != null)
            completedObject.SetActive(true);

        Debug.Log("solved!");
    }

    DragDrop pieceInSlot(DragDrop[] pieces)
    {
        foreach (DragDrop piece in pieces)
        {
            if (piece.item == correctDrop && isInSlot(piece))
                return piece;
        }

        return null;
    }
}
