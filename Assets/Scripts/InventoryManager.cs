using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [HideInInspector] static int MAX_STACK = 20;

    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    public int selectedSlot = 0;

    //Event system here for inventory updates to improve performance
    //and decrease dependencies
    public delegate void InventoryChange();

    public static event InventoryChange OnHeldItemChanged;

    bool inAction;

    private void Start()
    {
        ChangeSelectedSlot(0);
    }

    private void OnEnable()
    {
        PlayerActionController.OnActionStart += delegate () { inAction = true; };
        PlayerActionController.OnActionEnd += delegate () { inAction = false; };
    }

    private void OnDisable()
    {
        PlayerActionController.OnActionStart -= delegate () { inAction = true; };
        PlayerActionController.OnActionEnd -= delegate () { inAction = false; };
    }

    private void Update()
    {
        if (Input.inputString != null && !inAction)
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

            //Need this here for if first empty slot is the selected slot
            if (OnHeldItemChanged != null) OnHeldItemChanged();
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
        
        if(OnHeldItemChanged != null) OnHeldItemChanged();
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

    public bool Craft(Recipe recipe)
    {
        //check if player has enough of each type of ingredient to craft
        for (int ii = 0; ii < recipe.Ingredients.Length; ii++)
        {
            Item ingredient = recipe.Ingredients[ii];
            int count = recipe.Counts[ii];

            int found = 0;
            //check each inventory slot for specified item
            for (int jj = 0; jj < inventorySlots.Length; jj++)
            {
                InventorySlot slot = inventorySlots[jj];
                InventoryItem slotItem = slot.GetComponentInChildren<InventoryItem>();

                if (slotItem != null)
                {
                    if (slotItem.item == ingredient)
                    {
                        found += slotItem.count;
                        if (found >= count) jj = inventorySlots.Length;
                    }
                }
            }

            //if not enough resources found, cancel crafting
            if (found < count) return false;
        }

        //remove required ingredients
        for (int ii = 0; ii < recipe.Ingredients.Length; ii++)
        {
            Item ingredient = recipe.Ingredients[ii];
            int count = recipe.Counts[ii];

            //check each inventory slot for specified item
            for (int jj = 0; jj < inventorySlots.Length; jj++)
            {
                InventorySlot slot = inventorySlots[jj];
                InventoryItem slotItem = slot.GetComponentInChildren<InventoryItem>();

                if (slotItem != null)
                {
                    if (slotItem.item == ingredient)
                    {
                        if (count == slotItem.count)
                        {
                            Destroy(slotItem.gameObject);
                            count = 0;
                            jj = inventorySlots.Length;
                        }
                        else if (count > slotItem.count)
                        {
                            count -= slotItem.count;
                            Destroy(slotItem.gameObject);
                        }
                        else
                        {
                            slotItem.count -= count;
                            slotItem.RefreshCount();
                            count = 0;
                            jj = inventorySlots.Length;
                        }
                    }
                }
            }
        }

        //add crafted item
        if (recipe.output.type != ItemType.Upgrade)
        {
            //uses coroutines to ensure that prior destroys have finished
            StartCoroutine(GiveCrafted(recipe));
        }
        else
        {
            //item is an upgrade, handle accordingly
            //TODO: HANDLE ACCORDINGLY
        }
        return true;
    }

    private IEnumerator GiveCrafted(Recipe recipe)
    {
        yield return 0;
        bool res = AddItem(recipe.output);

        //if inventory is full, spawn crafted item on the ground
        if (!res)
        {
            //TODO: SPAWN ITEM ON GROUND
        }
    }
}
