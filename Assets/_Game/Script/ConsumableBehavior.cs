using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableBehavior : MonoBehaviour {
    private ItemObject itemObject => GetComponent<ItemObject>();
    private ConsumableItemSO consumableItem;
    private CharacterInventory inventory => FindObjectOfType<CharacterInventory>();

    private void Awake()
    {
        consumableItem = itemObject.Item as ConsumableItemSO;
    }

    public void CheckConsumableType(ConsumableType consumableType) {
        switch(consumableType)
        {
            case ConsumableType.Grenade:
                Debug.Log("Used Grenade");
                ThrowGrenade();
                break;
            case ConsumableType.HealthPotion:
                Debug.Log("Used Health Pot");
                ConsumeHealthPotion();
                break;
        }
    }

    #region Grenade Behavior
    private void ThrowGrenade()
    {
        transform.SetParent(null);
        Collider grenadeColl = GetComponent<Collider>();
        grenadeColl.enabled = true;
        grenadeColl.isTrigger = false;
        // Get the Rigidbody component of the grenade
        Rigidbody grenadeRB = GetComponent<Rigidbody>();
        grenadeRB.isKinematic = false;
        // Apply a force to the grenade to make it follow an arc trajectory
        Vector3 throwDirection = transform.forward.normalized; // Adjust this to set the direction of the throw
        grenadeRB.AddForce(throwDirection * consumableItem.throwForce, ForceMode.Impulse);

        Invoke("ExplodeGrenade", consumableItem.delayBeforeExplode);
    }

    private void ExplodeGrenade()
    {
        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, consumableItem.proximity);

        foreach (var collider in colliders)
        {
            HealthBehavior health = collider.GetComponent<HealthBehavior>();
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                if (health != null)
                {
                    health.TakeDamage(consumableItem.ConsumableAmount);
                }
                rb.AddExplosionForce(2000, gameObject.transform.position, consumableItem.proximity);
            }
        }
        GameObject explosion = Instantiate(consumableItem.consumableParticles, gameObject.transform.position, Quaternion.identity);
        Destroy(explosion, 2f);
        inventory.DestroyHeldItem();

    }
    #endregion

    #region HealthPotion Behavior
    private void ConsumeHealthPotion()
    {
        //Debug.Log("Healing for " + consumableItem.ConsumableAmount);
        IHealable healable = GameObject.FindGameObjectWithTag("Player").GetComponent<IHealable>();
        if (healable != null)
        {
            healable.Heal(consumableItem.ConsumableAmount);
            inventory.DestroyHeldItem();
        }
    }
    #endregion
}
