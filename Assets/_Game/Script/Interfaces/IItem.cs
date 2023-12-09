using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem {
    ItemType GetItemType();
    string GetItemName();
    string GetItemDescription();
}
