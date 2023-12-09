using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AIConfig")]
public class AIConfigSO : ScriptableObject {
    public bool hasGun;
    public EnemyType EnemyType;
    public float AISpeed;
    public float AIDamage;
    public float ProximityToPlayer;
    public float DistanceForAbility;
    public float AbilityDamageMultiplier;
}