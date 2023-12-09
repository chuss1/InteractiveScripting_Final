using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHand : MonoBehaviour {
    [SerializeField] private CharacterInventory inventory;

    [SerializeField] private float driftSpeed;
    [SerializeField] private float arrivalDistance = 0.1f; // The distance at which the key is considered arrived

    private Transform currentItemTransform;
    bool isItemMoving = false;

    private void OnEnable() {
        inventory.ItemAdded += ItemVisualUpdate;
        inventory.ItemDropped += DroppingItem;
    }

    private void OnDisable() {
        inventory.ItemAdded -= ItemVisualUpdate;
        inventory.ItemDropped -= DroppingItem;
    }

    private void ItemVisualUpdate() {
        //if(inventory.HoldingItem) return;
        ItemObject heldItem = inventory.GetHeldItem();
        if(currentItemTransform != heldItem.transform && isItemMoving) StopCoroutine(MoveKeyToTarget());

        isItemMoving = true;
        StartCoroutine(MoveKeyToTarget());
    }


    private void DroppingItem() {
        ItemObject heldItem = inventory.GetHeldItem();
        heldItem.transform.SetParent(null);
        if(heldItem.TryGetComponent(out Collider collider) && collider != null) {
            collider.enabled = true;
            collider.isTrigger = true;
        }
    }

    IEnumerator MoveKeyToTarget() {
        ItemObject heldItem = inventory.GetHeldItem();
        Transform itemTransform = heldItem.gameObject.transform; // Store the key's transform for convenience
        Quaternion originalRotation = itemTransform.rotation; // Store the original rotation

        if (itemTransform.TryGetComponent(out Collider collider) && collider != null)
        {
            collider.enabled = false;
        }

        while (Vector3.Distance(itemTransform.position, transform.position) > arrivalDistance)
        {
            Vector3 direction = (transform.position - itemTransform.position).normalized;
            itemTransform.position += direction * driftSpeed * Time.deltaTime;

            // Set the keycard object as a child of HeldObjectPos
            itemTransform.SetParent(transform);

            yield return null;
        }

        // Restore the original rotation after the movement
        itemTransform.rotation = transform.rotation;

        // When the key reaches the desired position, stop the coroutine
        isItemMoving = false;
    }

}
