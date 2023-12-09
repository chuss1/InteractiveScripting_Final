using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventory : MonoBehaviour
{
    [SerializeField] private CharacterRaycast characterRaycast;
    private CharacterAction characterAction => GetComponent<CharacterAction>();
    [SerializeField] private ItemObject heldItem;
    private bool isHoldingItem = false;

    public Action ItemAdded;
    public Action ItemDropped;
    

    private void OnEnable() {
        characterRaycast.OnItemPickup += AddItem;
    }

    private void OnDisable() {
        characterRaycast.OnItemPickup -= AddItem;
    }

    public void CheckItemDropped() {
        if(isHoldingItem) {
            ItemDropped?.Invoke();
            ItemUsed();
        }
    }

    private void AddItem(ItemObject item) {
        if(item != null && !isHoldingItem) {
            heldItem = item;
            isHoldingItem = true;
            Debug.Log("Now holding " + item.Item.GetItemName());
            ItemAdded?.Invoke();
        }
    }

    public void ItemUsed() {
        heldItem = null;
        isHoldingItem = false;
    }

    public ItemObject GetHeldItem() {
        return heldItem;
    }

    public void DestroyHeldItem() {
        Destroy(heldItem.gameObject);
        ItemUsed();
    }
}
