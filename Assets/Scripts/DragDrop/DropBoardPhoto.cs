using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropBoardPhoto : MonoBehaviour, IDropHandler, IDropSlot
{
    public string correctDrop;
    public GameObject completedObject;

    private bool correct = false;
    public static int correctBoxes = 0;
    private DragDrop occupant;
    private static readonly List<DropBoardPhoto> allSlots = new List<DropBoardPhoto>();
    private static bool photosShuffled = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetSlots()
    {
        allSlots.Clear();
        photosShuffled = false;
    }

    private void Awake()
    {
        allSlots.Add(this);
    }

    private void Start()
    {
        if (photosShuffled)
            return;

        photosShuffled = true;

        List<DragDrop> photos = new List<DragDrop>();
        foreach (DragDrop piece in transform.parent.GetComponentsInChildren<DragDrop>())
        {
            if (piece.slot == null)
                photos.Add(piece);
        }

        List<Vector2> spots = new List<Vector2>();
        foreach (DragDrop photo in photos)
            spots.Add(photo.StartPosition);

        for (int i = spots.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Vector2 temp = spots[i];
            spots[i] = spots[j];
            spots[j] = temp;
        }

        for (int i = 0; i < photos.Count; i++)
            photos[i].SetStartPosition(spots[i]);
    }

    private void OnDestroy()
    {
        allSlots.Remove(this);
    }

    void LockAll()
    {
        foreach (DropBoardPhoto slot in allSlots)
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
            correctBoxes++;
            Debug.Log("Correct! " + correctBoxes);

            if (correctBoxes == FindObjectsByType<DropBoardPhoto>(FindObjectsSortMode.None).Length)
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

        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
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
        correctBoxes--;
        Debug.Log("Taken away! " + correctBoxes);
    }
}
