using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingMenu : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Recipe[] recipes;
    public bool[] disabled;

    public void Craft(int id)
    {
        Recipe recipe = recipes[id];
        if (recipe.output.type != ItemType.Upgrade || disabled[id] == false)
        {
            bool result = inventoryManager.Craft(recipes[id]);

            if(recipe.output.type == ItemType.Upgrade)
            {
                disabled[id] = result;
            }
        }
    }
}
