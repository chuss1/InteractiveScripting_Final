using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[DisallowMultipleComponent]
public class CharacterAction : MonoBehaviour {
    [SerializeField] private CharacterInput input;
    [SerializeField] private GunSelector gunSelector;
    [SerializeField] private CharacterRaycast raycast;
    [SerializeField] private CharacterInventory inventory;

    public Action WeaponFired;

    private void OnEnable() {
        input.fireWeapon += FireWeapon;
        input.interact += CheckRayCast;
        input.dropItem += DropItem;
        input.useConsumable += UseConsumable;
        input.switchWeapon += SwitchWeapon;
    }

    private void OnDisable() {
        input.fireWeapon -= FireWeapon;
        input.interact -= CheckRayCast;
        input.dropItem -= DropItem;
        input.useConsumable -= UseConsumable;
        input.switchWeapon -= SwitchWeapon;
    }

    private void FireWeapon() {
        WeaponFired?.Invoke();
    }

    private void SwitchWeapon() {
        Debug.Log("Switch Weapon Input Pressed");
        if(gunSelector != null) {
            gunSelector.SwitchWeapon();
        }
    }

    private void CheckRayCast() {
        raycast.CastInteractRay();
    }

    private void DropItem() {
        inventory.CheckItemDropped();
    }

    private void UseConsumable() {
        if(inventory.GetHeldItem() != null) {
            ItemSO itemSO = inventory.GetHeldItem().Item;

            if(itemSO is ConsumableItemSO consumableItem) {
                ConsumableBehavior consumableBehavior = inventory.GetHeldItem().GetComponent<ConsumableBehavior>();
                consumableBehavior.CheckConsumableType(consumableItem.ConsumableType);
                //characterInventory.DestroyHeldItem();
            } else {
                Debug.Log("Not A Consumable Item");
                return;
            }
        }
    }
}
