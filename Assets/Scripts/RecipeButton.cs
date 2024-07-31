using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeButton : MonoBehaviour
{
    public CraftingMenu CraftingMenu;
    public int Index;
    public TMP_Text Title;
    public TMP_Text IngredientList;

    [HideInInspector] public Recipe recipe;

    // Start is called before the first frame update
    void Start()
    {
        recipe = CraftingMenu.recipes[Index];

        Title.text = recipe.output.name;

        IngredientList.text = "";
        for(int ii = 0; ii < recipe.Ingredients.Length; ii++)
        {
            IngredientList.text += recipe.Ingredients[ii].name + " x" + recipe.Counts[ii].ToString() + "\n";
        }
    }

    public void Craft()
    {
        CraftingMenu.Craft(Index);
    }
}
