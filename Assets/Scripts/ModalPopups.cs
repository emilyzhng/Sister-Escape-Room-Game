using UnityEngine;
using UnityEngine.UI;

// While a popup is open, only the popup can be clicked: each popup gets an
// invisible full-screen blocker behind its own content.
public class ModalPopups : MonoBehaviour
{
    public Transform[] popupGroups;
    public float blockerSize = 20000f;

    private void Awake()
    {
        foreach (Transform popup in transform)
        {
            if (System.Array.IndexOf(popupGroups, popup) >= 0)
            {
                foreach (Transform grouped in popup)
                    addBlocker(grouped);
            }
            else
            {
                addBlocker(popup);
            }
        }
    }

    void addBlocker(Transform popup)
    {
        GameObject blocker = new GameObject("ClickBlocker", typeof(RectTransform));
        blocker.transform.SetParent(popup, false);
        blocker.transform.SetAsFirstSibling();

        RectTransform rect = blocker.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(blockerSize, blockerSize);

        Image image = blocker.AddComponent<Image>();
        image.color = new Color(0f, 0f, 0f, 0f);
    }
}
