using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Drag a medal and let go near a knob to hang it there.
// The medal's pivot (top of the ribbon) snaps onto the knob.
// Dropping on a taken knob swaps the two medals; dropping elsewhere puts it back.
[RequireComponent(typeof(CanvasGroup))]
public class MedalDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,
    IPointerEnterHandler, IPointerExitHandler
{
    public MedalKnob startKnob;     // used when shuffleOnStart is off
    public bool shuffleOnStart = true;
    public float snapDistance = 400f;

    [HideInInspector] public MedalKnob knob;
    [HideInInspector] public bool locked = false;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    // runs the first time the medal view opens
    private void Start()
    {
        // another medal already shuffled everyone
        if (knob != null)
            return;

        if (shuffleOnStart)
            shuffleMedals();
        else if (startKnob != null)
            SnapTo(startKnob);
    }

    // hang every medal on a random knob, never already in the solved order
    void shuffleMedals()
    {
        MedalDrag[] medals = transform.parent.GetComponentsInChildren<MedalDrag>();
        List<MedalKnob> knobs = new List<MedalKnob>(transform.parent.GetComponentsInChildren<MedalKnob>());

        for (int attempt = 0; attempt < 100; attempt++)
        {
            for (int i = knobs.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                MedalKnob temp = knobs[i];
                knobs[i] = knobs[j];
                knobs[j] = temp;
            }

            for (int i = 0; i < medals.Length && i < knobs.Count; i++)
                medals[i].SnapTo(knobs[i]);

            if (!medalsInOrder())
                return;
        }
    }

    // hand cursor over the medal, and kept for the whole drag
    public void OnPointerEnter(PointerEventData eventData)
    {
        HandCursor.Show(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!eventData.dragging)
            HandCursor.Hide(this);
    }

    private void OnDisable()
    {
        HandCursor.Hide(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (locked)
        {
            // cancel the drag
            eventData.pointerDrag = null;
            return;
        }

        HandCursor.Show(this);
        canvasGroup.blocksRaycasts = false;
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // the medal may snap away from the mouse; if the mouse is still over it,
        // OnPointerEnter turns the hand back on next frame
        HandCursor.Hide(this);

        MedalKnob fromKnob = knob;
        MedalKnob target = findNearestKnob();

        if (target == null || target == fromKnob)
        {
            SnapTo(fromKnob);
            return;
        }

        if (target.medal != null)
            target.medal.SnapTo(fromKnob);

        SnapTo(target);
        checkSolved();
    }

    void checkSolved()
    {
        if (!medalsInOrder())
            return;

        // lock the medals; their fronts stay visible as the clue for the next puzzle
        foreach (MedalKnob k in transform.parent.GetComponentsInChildren<MedalKnob>())
            k.medal.locked = true;

        MedalPuzzle puzzle = GetComponentInParent<MedalPuzzle>();
        if (puzzle != null)
            puzzle.Solved();
        else
            Debug.Log("Medals solved!");
    }

    // true when every knob has a medal and the numbers increase left to right
    bool medalsInOrder()
    {
        List<MedalKnob> knobs = new List<MedalKnob>(transform.parent.GetComponentsInChildren<MedalKnob>());
        knobs.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));

        int previous = int.MinValue;

        foreach (MedalKnob k in knobs)
        {
            if (k.medal == null)
                return false;

            int number = k.medal.GetComponentInChildren<MedalFlip>().Number();

            if (number <= previous)
                return false;

            previous = number;
        }

        return true;
    }

    public void SnapTo(MedalKnob newKnob)
    {
        if (newKnob == null)
            return;

        if (knob != null && knob.medal == this)
            knob.medal = null;

        knob = newKnob;
        knob.medal = this;
        rectTransform.position = knob.transform.position;
    }

    MedalKnob findNearestKnob()
    {
        MedalKnob nearest = null;
        float best = snapDistance * canvas.scaleFactor;

        foreach (MedalKnob candidate in transform.parent.GetComponentsInChildren<MedalKnob>())
        {
            float distance = Vector2.Distance(rectTransform.position, candidate.transform.position);

            if (distance < best)
            {
                best = distance;
                nearest = candidate;
            }
        }

        return nearest;
    }
}
