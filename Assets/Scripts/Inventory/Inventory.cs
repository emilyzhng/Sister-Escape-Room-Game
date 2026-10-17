using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<string> inventoryItems = new List<string>();

    // tells InventoryUI to redraw
    public event System.Action Changed;

    public void AddItem(string item)
    {
        inventoryItems.Add(item);
        Changed?.Invoke();
    }

    public void RemoveItem(string item)
    {
        if (inventoryItems.Contains(item))
        {
            inventoryItems.Remove(item);
            Changed?.Invoke();
        }
    }

    public bool HasItem(string item)
    {
        return inventoryItems.Contains(item);
    }
}