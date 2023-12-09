using UnityEngine;
using System;

public class PanelBehavior : MonoBehaviour, IInteract
{
    [SerializeField] private KeyType PanelKeyType;
    [SerializeField] private CharacterInventory inventory => FindObjectOfType<CharacterInventory>();
    [SerializeField] private AudioSource PanelPingSFX;
    private Animator animator => GetComponent<Animator>();
    public Action<bool> OnPanelActivated;
    public bool isUnlocked;

    private void Start()
    {
        PanelPingSFX = GetComponent<AudioSource>();
        isUnlocked = false;
    }


    private void PanelActivate(ItemObject heldItem) {
        if (heldItem != null) {
            ItemSO itemSO = heldItem.Item;

            // Check if itemSO is a KeyItemSO
            if (itemSO is KeyItemSO keyItem) {
                // It's a KeyItemSO, you can access its properties
                if (keyItem.GetKeyType() == PanelKeyType) {
                    // Implement the unlocking logic here
                    isUnlocked = true;
                    OnPanelActivated?.Invoke(true);
                    PanelPingSFX.Play();
                    animator.SetTrigger("KeyUsed");
                    inventory.DestroyHeldItem();
                } else {
                    Debug.Log("Wrong Key for This door!");
                }
            } else {
                Debug.Log("Not A key Item");
            }
        }
    }

    public void OnInteract() {
        if(isUnlocked) return;

        PanelActivate(inventory.GetHeldItem());
    }
}
