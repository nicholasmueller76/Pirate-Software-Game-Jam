using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable Object/Item")]
public class Item : ScriptableObject
{
    public Sprite image;
    public ItemType type;
    public bool stackable;
}

public enum ItemType
{
    Swingable,
    Throwable,
    Consumable,
    Material,
    Upgrade
}