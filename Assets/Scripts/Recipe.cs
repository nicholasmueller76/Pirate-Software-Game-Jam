using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "Scriptable Object/Recipe")]

public class Recipe : ScriptableObject
{
    public Item output;
    public int shadowCost;

    public Item[] Ingredients;
    public int[] Counts;
}