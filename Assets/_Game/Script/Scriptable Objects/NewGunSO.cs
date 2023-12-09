using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGun", menuName = "Guns/NewGun")]
public class NewGunSO : ScriptableObject {
    public bool forPlayer;
    public GunType Type;
    public string Name;
    public Vector3 SpawnPoint;
    public Vector3 SpawnRotation;
    public DamageConfigSO damageConfig;
    public ShootConfigSO shootConfig;
    public TrailConfigSO trailConfig;
}
