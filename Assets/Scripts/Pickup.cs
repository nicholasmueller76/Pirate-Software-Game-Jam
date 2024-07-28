using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField]
    Item itemData;

    public Item GetItemData()
    {
        return itemData;
    }
}
