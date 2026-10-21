using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Shows what the player is carrying as a column of slots down the left edge of the screen.
// Items with the same display name share one slot with a count; gold coins always get one slot each at the top.
public class InventoryUI : MonoBehaviour
{
    [System.Serializable]
    public class ItemLook
    {
        public string item;          // name used in Inventory.AddItem
        public string displayName;   // items with the same display name stack in one slot
        public Sprite icon;          // optional; a placeholder is drawn when empty
        public bool isCoin;          // placeholder look: gold coin instead of a labelled box
    }

    public Inventory inventory;
    public Canvas canvas;

    public List<ItemLook> looks = new List<ItemLook>
    {
        new ItemLook { item = "screwdriver", displayName = "Screwdriver" },
        new ItemLook { item = "BookCoin", displayName = "Gold Coin", isCoin = true },
        new ItemLook { item = "StandCoin", displayName = "Gold Coin", isCoin = true },
        new ItemLook { item = "DrawerCoin", displayName = "Gold Coin", isCoin = true },
        new ItemLook { item = "ClosetKey", displayName = "Closet Key" },
        new ItemLook { item = "Toy", displayName = "Toy" },
        new ItemLook { item = "DoorKey", displayName = "Door Key" },
        new ItemLook { item = "MusicSheet", displayName = "Music Sheet" },
    };

    [System.Serializable]
    public class ViewableItem
    {
        public string item;
        public Sprite image;
    }

    [System.Serializable]
    public class ItemIcon
    {
        public string item;
        public Sprite icon;
    }

    [Header("Inventory icons")]
    public List<ItemIcon> icons = new List<ItemIcon>();

    [Header("Items the player can click to look at")]
    public List<ViewableItem> viewables = new List<ViewableItem>();

    [Header("Coin goal (shown after the crow asks for them)")]
    public List<string> goalCoins = new List<string> { "BookCoin", "StandCoin", "DrawerCoin" };
    public Color missingCoinColor = new Color(0.55f, 0.55f, 0.55f, 0.8f);

    [Header("Shapes for slots and placeholder icons (Unity's built-in UISprite and Knob)")]
    public Sprite roundedBox;
    public Sprite circle;

    [Header("Layout (canvas units, from the screen center)")]
    public Vector2 firstSlotPosition = new Vector2(-2140f, 1380f);
    public float slotSize = 260f;
    public float slotSpacing = 30f;

    private RectTransform bar;
    private readonly List<GameObject> slots = new List<GameObject>();
    private bool showCoinGoal = false;
    private bool coinGoalDone = false;
    private readonly HashSet<string> coinsFound = new HashSet<string>();
    private GameObject viewer;
    private Image viewerImage;

    private void Start()
    {
        if (inventory == null)
            inventory = GetComponent<Inventory>();
        if (canvas == null)
            canvas = FindFirstObjectByType<Canvas>();

        // last child of the canvas, so it is drawn on top of rooms and popups
        GameObject barObject = new GameObject("InventoryBar", typeof(RectTransform));
        bar = barObject.GetComponent<RectTransform>();
        bar.SetParent(canvas.transform, false);
        bar.SetAsLastSibling();

        inventory.Changed += Refresh;
        Refresh();
    }

    private void OnDestroy()
    {
        if (inventory != null)
            inventory.Changed -= Refresh;
    }

    public void ShowCoinGoal()
    {
        if (showCoinGoal || coinGoalDone)
            return;

        showCoinGoal = true;
        Refresh();
    }

    public void EndCoinGoal()
    {
        coinGoalDone = true;
        showCoinGoal = false;
        Refresh();
    }

    void Refresh()
    {
        foreach (GameObject slot in slots)
            Destroy(slot);
        slots.Clear();

        foreach (string item in inventory.inventoryItems)
        {
            if (goalCoins.Contains(item))
                coinsFound.Add(item);
        }

        int index = 0;

        foreach (string coin in goalCoins)
        {
            if (showCoinGoal)
                slots.Add(createSlot(findLook(coin), 1, index++, !coinsFound.Contains(coin)));
            else if (inventory.HasItem(coin))
                slots.Add(createSlot(findLook(coin), 1, index++, false));
        }

        // group by display name, in the order things were picked up
        List<string> order = new List<string>();
        Dictionary<string, int> counts = new Dictionary<string, int>();
        Dictionary<string, ItemLook> lookFor = new Dictionary<string, ItemLook>();

        foreach (string item in inventory.inventoryItems)
        {
            if (goalCoins.Contains(item))
                continue;

            ItemLook look = findLook(item);

            if (!counts.ContainsKey(look.displayName))
            {
                order.Add(look.displayName);
                counts[look.displayName] = 0;
                lookFor[look.displayName] = look;
            }

            counts[look.displayName]++;
        }

        for (int i = 0; i < order.Count; i++)
            slots.Add(createSlot(lookFor[order[i]], counts[order[i]], index++, false));
    }

    Sprite iconFor(string item)
    {
        foreach (ItemIcon itemIcon in icons)
        {
            if (itemIcon.item == item)
                return itemIcon.icon;
        }

        return null;
    }

    Sprite viewImageFor(string item)
    {
        foreach (ViewableItem viewable in viewables)
        {
            if (viewable.item == item)
                return viewable.image;
        }

        return null;
    }

    public void ShowView(Sprite image)
    {
        if (viewer == null)
        {
            viewer = newUI("ItemViewer", canvas.transform);
            RectTransform rect = viewer.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = Vector2.zero;
            Image dim = viewer.AddComponent<Image>();
            dim.color = new Color(0f, 0f, 0f, 0.75f);
            Button close = viewer.AddComponent<Button>();
            close.transition = Selectable.Transition.None;
            close.onClick.AddListener(() => viewer.SetActive(false));

            GameObject picture = newUI("Image", rect);
            RectTransform pictureRect = picture.GetComponent<RectTransform>();
            pictureRect.anchorMin = new Vector2(0.05f, 0.05f);
            pictureRect.anchorMax = new Vector2(0.95f, 0.95f);
            pictureRect.sizeDelta = Vector2.zero;
            viewerImage = picture.AddComponent<Image>();
            viewerImage.preserveAspect = true;
            viewerImage.raycastTarget = false;
        }

        viewerImage.sprite = image;
        viewer.transform.SetAsLastSibling();
        viewer.SetActive(true);
    }

    ItemLook findLook(string item)
    {
        foreach (ItemLook look in looks)
        {
            if (look.item == item)
                return look;
        }

        return new ItemLook { item = item, displayName = item };
    }

    GameObject createSlot(ItemLook look, int count, int index, bool missing)
    {
        // slot background
        GameObject slot = newUI("Slot " + look.displayName, bar);
        RectTransform rect = slot.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(slotSize, slotSize);
        rect.anchoredPosition = firstSlotPosition - new Vector2(0f, index * (slotSize + slotSpacing));
        Image background = slot.AddComponent<Image>();
        background.sprite = roundedBox;
        background.type = Image.Type.Sliced;
        background.color = new Color(0.1f, 0.1f, 0.15f, 0.75f);
        background.raycastTarget = false;

        // icon: real sprite, or a placeholder
        GameObject iconObject = newUI("Icon", rect);
        RectTransform iconRect = iconObject.GetComponent<RectTransform>();
        iconRect.sizeDelta = new Vector2(slotSize * 0.75f, slotSize * 0.75f);
        Image icon = iconObject.AddComponent<Image>();
        icon.raycastTarget = false;

        Sprite view = missing ? null : viewImageFor(look.item);
        if (view != null)
        {
            background.raycastTarget = true;
            Button open = slot.AddComponent<Button>();
            open.transition = Selectable.Transition.None;
            open.onClick.AddListener(() => ShowView(view));
            slot.AddComponent<HandCursorOnHover>();
        }

        Sprite iconSprite = look.icon != null ? look.icon : iconFor(look.item);
        if (iconSprite == null)
            iconSprite = view;
        if (iconSprite != null)
        {
            icon.sprite = iconSprite;
            icon.preserveAspect = true;
            if (missing)
                icon.color = missingCoinColor;
        }
        else if (look.isCoin)
        {
            icon.sprite = circle;
            icon.color = missing ? missingCoinColor : new Color(0.95f, 0.77f, 0.25f);
            addText(iconRect, "$", slotSize * 0.4f,
                missing ? new Color(0.35f, 0.35f, 0.35f) : new Color(0.45f, 0.3f, 0.05f));
        }
        else
        {
            icon.sprite = roundedBox;
            icon.type = Image.Type.Sliced;
            icon.color = new Color(0.9f, 0.9f, 0.85f);
            addText(iconRect, look.displayName, slotSize * 0.13f, new Color(0.15f, 0.15f, 0.2f));
        }

        // count badge for stacked items
        if (count > 1)
        {
            TMP_Text badge = addText(rect, "x" + count, slotSize * 0.2f, Color.white);
            badge.alignment = TextAlignmentOptions.BottomRight;
            badge.margin = new Vector4(0f, 0f, slotSize * 0.06f, slotSize * 0.02f);
        }

        return slot;
    }

    GameObject newUI(string name, Transform parent)
    {
        GameObject go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        return go;
    }

    TMP_Text addText(RectTransform parent, string text, float size, Color color)
    {
        GameObject textObject = newUI("Text", parent);
        RectTransform rect = textObject.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;

        TextMeshProUGUI label = textObject.AddComponent<TextMeshProUGUI>();
        label.text = text;
        label.fontSize = size;
        label.color = color;
        label.fontStyle = FontStyles.Bold;
        label.alignment = TextAlignmentOptions.Center;
        label.raycastTarget = false;
        return label;
    }
}
