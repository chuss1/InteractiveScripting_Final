using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRaycast : MonoBehaviour {
    [SerializeField] private float raycastRange;
    [SerializeField] private LayerMask interactLayer;
    [SerializeField] private Transform raycastStart;

    public Action<ItemObject> OnItemPickup;
    //public Action<ItemObject> OnInteracted;

    public void CastInteractRay() {
        if(Physics.Raycast(raycastStart.position, transform.forward, out RaycastHit hit, raycastRange, interactLayer)) {
            if(hit.transform.TryGetComponent(out IInteract interactable) && interactable != null) {
                interactable.OnInteract(); // For interacting with something like the panel behavior
            } else if( hit.transform.TryGetComponent(out ItemObject item) && item != null) {
                OnItemPickup?.Invoke(item); // For Items
            }
        }
    }
}
