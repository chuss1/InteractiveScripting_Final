using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KeyItem", menuName = "Items/New Key Item", order = 2)]
public class KeyItemSO : ItemSO, IItemWithKeyType {
    public KeyType KeyType;

    public KeyType GetKeyType()
    {
        return KeyType;
    }
}
