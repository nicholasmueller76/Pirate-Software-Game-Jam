using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [HideInInspector] static int MAX_STACK = 20;
    [HideInInspector] static int HOTBAR_SLOTS = 7;

    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    public int selectedSlot = 0;

    private void Start()
    {
        ChangeSelectedSlot(0);
    }

    private void Update()
    {
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 8)
            {
                ChangeSelectedSlot(number - 1);
            }
        }
    }
    public bool AddItem(Item item)
    {
        // returns true if item is successfully added to the player's inventory
        // returns false if the player's inventory is full

        InventorySlot freeSlot = null;

        //check for slots with less than the maximum number of the same item
        for (int ii = 0; ii < inventorySlots.Length; ii++)
        {
            InventorySlot slot = inventorySlots[ii];
            InventoryItem slotItem = slot.GetComponentInChildren<InventoryItem>();

            if(item.stackable && slotItem != null && slotItem.item == item && slotItem.count < MAX_STACK)
            {
                slotItem.count++;
                slotItem.RefreshCount();
                return true;
            }

            if (freeSlot == null && slotItem == null) //empty slot found
            {
                freeSlot = slot;
            }
        }

        //if not yet placed, place in the first empty slot
        if(freeSlot != null)
        {
            SpawnNewItem(item, freeSlot);
            return true;
        }

        //inventory full
        return false;
    }
    private void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItem = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item);
    }

    public void ChangeSelectedSlot(int newValue)
    {
        if(selectedSlot != -1) inventorySlots[selectedSlot].Deselect();
        inventorySlots[newValue].Select();
        selectedSlot = newValue;
    }

    public Item GetSelectedItem()
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem item = slot.GetComponentInChildren<InventoryItem>();
        if(item != null)
        {
            return item.item;
        }
        else
        {
            return null;
        }
    }
}
