using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable Object/Item")]
public class Item : ScriptableObject
{
    public Sprite image;
    public ItemType type;
    public bool stackable;
    public float maxStack;
    public float damage;
    public float heal;
    public float throwForce;
    public bool canMineTrees;
    public bool canMineRocks;
    public GameObject throwableItemPrefab;
}

public enum ItemType
{
    Swingable,
    Throwable,
    Consumable,
    Material,
    Upgrade
}