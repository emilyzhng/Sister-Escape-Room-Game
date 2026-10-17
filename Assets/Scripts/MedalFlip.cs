using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

// Hover over a medal to flip it and see the clue on its back;
// move the mouse away and it flips back.
public class MedalFlip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Back clue (backText is also the number used for the order check)")]
    public string backText = "1";
    public bool showNumberText = true;   // turn off if the back image already shows the number
    public float backFontSize = 220f;
    public Color backTextColor = new Color(0.25f, 0.18f, 0.05f);

    [Header("Back side (front is the Image's Source Image)")]
    public Sprite backSprite;
    public Color backColor = new Color(0.93f, 0.76f, 0.3f, 1f);   // only used when there is no back image

    public float flipDuration = 0.3f;

    private Image image;
    private Sprite frontSprite;
    private Color frontColor;
    private TMP_Text clueText;
    private bool showingBack = false;
    private bool wantBack = false;
    private bool isFlipping = false;

    private void Awake()
    {
        image = GetComponent<Image>();
        frontSprite = image.sprite;
        frontColor = image.color;

        // real artwork: keep its shape and colors instead of the placeholder look
        if (backSprite != null)
            image.preserveAspect = true;

        createClueText();
        showSide(false);
    }

    private void OnDisable()
    {
        // leaving the view: reset to the front instantly
        isFlipping = false;
        wantBack = false;
        showingBack = false;
        showSide(false);
        setScaleX(1f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        flipTo(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        flipTo(false);
    }

    public int Number()
    {
        int number;
        return int.TryParse(backText, out number) ? number : 0;
    }

    void flipTo(bool back)
    {
        wantBack = back;

        // a running flip picks up the new target when it finishes
        if (!isFlipping && showingBack != wantBack)
            StartCoroutine(Flip());
    }

    IEnumerator Flip()
    {
        isFlipping = true;
        float half = flipDuration / 2f;

        while (showingBack != wantBack)
        {
            yield return ScaleX(1f, 0f, half);

            showingBack = !showingBack;
            showSide(showingBack);

            yield return ScaleX(0f, 1f, half);
        }

        isFlipping = false;
    }

    IEnumerator ScaleX(float from, float to, float duration)
    {
        float t = 0;

        while (t < duration)
        {
            // unscaled so the flip still animates if the game is paused (timeScale 0)
            t += Time.unscaledDeltaTime;
            setScaleX(Mathf.Lerp(from, to, t / duration));
            yield return null;
        }

        setScaleX(to);
    }

    void setScaleX(float x)
    {
        Vector3 scale = transform.localScale;
        scale.x = x;
        transform.localScale = scale;
    }

    void showSide(bool back)
    {
        if (back && backSprite != null)
        {
            // real artwork: show it in its own colors
            image.sprite = backSprite;
            image.color = Color.white;
        }
        else
        {
            image.sprite = frontSprite;
            image.color = back ? backColor : frontColor;
        }

        clueText.gameObject.SetActive(back && showNumberText);
    }

    void createClueText()
    {
        GameObject textObject = new GameObject("BackText", typeof(RectTransform));
        textObject.transform.SetParent(transform, false);

        RectTransform rect = textObject.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;

        clueText = textObject.AddComponent<TextMeshProUGUI>();
        clueText.text = backText;
        clueText.fontSize = backFontSize;
        clueText.color = backTextColor;
        clueText.fontStyle = FontStyles.Bold;
        clueText.alignment = TextAlignmentOptions.Center;
        clueText.raycastTarget = false;
    }
}
