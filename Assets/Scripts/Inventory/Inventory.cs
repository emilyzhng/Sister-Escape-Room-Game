using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<string> inventoryItems = new List<string>();

    public void AddItem(string item)
    {
        inventoryItems.Add(item);
    }

    public void RemoveItem(string item)
    {
        if (inventoryItems.Contains(item))
        {
            inventoryItems.Remove(item);
        }
    }

    public bool HasItem(string item)
    {
        return inventoryItems.Contains(item);
    }
}