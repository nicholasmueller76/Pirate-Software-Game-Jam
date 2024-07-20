using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugItemSpawner : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Item[] items;

    public void PickupItem(int id)
    {
        bool result = inventoryManager.AddItem(items[id]);
    }
}
