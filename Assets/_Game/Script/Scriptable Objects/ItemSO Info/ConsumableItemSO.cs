using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConsumableItem", menuName = "Items/New Consumable Item", order = 1)]
public class ConsumableItemSO : ItemSO {
    public ConsumableType ConsumableType;

    public float ConsumableAmount;
    public GameObject consumableParticles;
    public float proximity;
    public float throwForce;
    [Range(0f, 5f)] public float delayBeforeExplode;
}
