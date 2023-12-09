using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Items/New Item", order = 0)]
public class ItemSO : ScriptableObject, IItem {
    public ItemType ItemType;
    public string ItemName;

    [TextArea(5,15)]
    public string ItemDescription;

    public ItemType GetItemType()
    {
        return ItemType;
    }
    public string GetItemName()
    {
        return ItemName;
    }
    public string GetItemDescription()
    {
        return ItemDescription;
    }



}
