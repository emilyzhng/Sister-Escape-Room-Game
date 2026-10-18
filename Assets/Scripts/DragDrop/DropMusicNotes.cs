using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropMusicNotes : MonoBehaviour, IDropHandler
{
    public string correctDrop;
    public GameObject completedObject;
    public bool need;

    private static readonly List<DropMusicNotes> answerSpots = new List<DropMusicNotes>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void resetBetweenPlays()
    {
        answerSpots.Clear();
    }

    private void Awake()
    {
        if (need)
            answerSpots.Add(this);
    }

    private void OnDestroy()
    {
        answerSpots.Remove(this);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        DragDrop item = eventData.pointerDrag.GetComponent<DragDrop>();

        if (item == null)
            return;

        eventData.pointerDrag.GetComponent<RectTransform>().position =
            GetComponent<RectTransform>().position;

        Debug.Log((need && item.item == correctDrop ? "Correct spot: " : "Wrong spot: ") +
            transform.parent.name + "/" + name);

        checkSolved();
    }

    void checkSolved()
    {
        DragDrop[] pieces = FindObjectsByType<DragDrop>(FindObjectsSortMode.None);
        int filled = 0;

        foreach (DropMusicNotes spot in answerSpots)
        {
           if (spot.hasNote(pieces))
           {
                filled++;
                Debug.Log(filled);
           }
        }

       Debug.Log("Notes in place: " + filled + " / " + answerSpots.Count);
 
        if (answerSpots.Count > 0 && filled == answerSpots.Count)
        {
            if (completedObject != null)
                completedObject.SetActive(true);

            Debug.Log("solved!");
        }
    }

    bool hasNote(DragDrop[] pieces)
    {
        foreach (DragDrop piece in pieces)
        {
            if (piece.item == correctDrop &&
                Vector2.Distance(piece.transform.position, transform.position) < 1f)
                return true;
        }

        return false;
    }
}
