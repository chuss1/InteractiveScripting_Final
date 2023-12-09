using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class GunObject : MonoBehaviour, IGunActions {
    public NewGunSO gun;
    public int currentAmmo;
    private bool isSpawned = false;
    [SerializeField]private CharacterAction actions;
    public AudioSource gunShotSFX;
    public Animator animator;

    private float LastShootTime;
    private ParticleSystem ShootSystem => GetComponentInChildren<ParticleSystem>();
    private ObjectPool<TrailRenderer> TrailPool;
    private bool isReloading;
    public Action<int, GunObject> WeaponFired;
    
    private void Awake() {
        isSpawned = false;
    }
    private void OnEnable() {
        if(gun.forPlayer) {
            actions = FindObjectOfType<CharacterAction>();
            actions.WeaponFired += Shoot;
        }
        currentAmmo = gun.damageConfig.MaxAmmo;
    }

    private void OnDisable() {
        if(gun.forPlayer) {
            actions.WeaponFired -= Shoot;
        }
    }

    public void Spawn(Transform parent) {
        if(!isSpawned) {
            if (gun == null) {
                Debug.LogError("Gun object is null!");
                return; // Abort the method if gun is null
            }

            Debug.Log("Spawning " + gun.name);
            gameObject.SetActive(true);
            currentAmmo = gun.damageConfig.MaxAmmo;
            isReloading = false;
            LastShootTime = 0;
            TrailPool = new ObjectPool<TrailRenderer>(CreateTrail);

            transform.SetParent(parent, false);
            transform.localPosition = gun.SpawnPoint;
            transform.localRotation = Quaternion.Euler(gun.SpawnRotation);
            gunShotSFX = GetComponent<AudioSource>();
            animator = GetComponent<Animator>();

            if (gunShotSFX == null) {
                Debug.LogError("gunShotSFX is null! Make sure it's properly assigned.");
                return;
            }

            WeaponFired?.Invoke(currentAmmo, this);
        }
        
        isSpawned = true;
    }

    public void Shoot() {
        if(Time.time > gun.shootConfig.FireRate + LastShootTime && !isReloading) {
            LastShootTime = Time.time;
            ShootSystem.Play();
            currentAmmo--;
            WeaponFired?.Invoke(currentAmmo, this);
            gunShotSFX.Play();
            animator.SetTrigger("GunShoot");
            if(currentAmmo == 0) isReloading = true;

            Vector3 shootDirection = ShootSystem.transform.forward + new Vector3(UnityEngine.Random.Range(-gun.shootConfig.Spread.x, gun.shootConfig.Spread.x), 
                                                                                 UnityEngine.Random.Range(-gun.shootConfig.Spread.y, gun.shootConfig.Spread.y), 
                                                                                 UnityEngine.Random.Range(-gun.shootConfig.Spread.z, gun.shootConfig.Spread.z));

            shootDirection.Normalize();

            if(Physics.Raycast(ShootSystem.transform.position, shootDirection, out RaycastHit hit, float.MaxValue, gun.shootConfig.HitMask)) {
                StartCoroutine(PlayTrail(ShootSystem.transform.position, hit.point, hit));
            } else {
                StartCoroutine(PlayTrail(ShootSystem.transform.position, ShootSystem.transform.position + (shootDirection * gun.trailConfig.MissDistance), new RaycastHit()));
            }
            if(currentAmmo == 0) {
                StartCoroutine(ResetCooldown());
            }
        }
    }

    public IEnumerator PlayTrail(Vector3 startPoint, Vector3 endPoint, RaycastHit hit)
    {
        TrailRenderer instance = TrailPool.Get();
        instance.gameObject.SetActive(true);
        instance.transform.position = startPoint;

        yield return null; // avoid position carry-over from last frame if reused

        instance.emitting = true;

        float distance = Vector3.Distance(startPoint, endPoint);
        float remainingDistance = distance;
        while (remainingDistance > 0)
        {
            instance.transform.position = Vector3.Lerp(startPoint, endPoint, Mathf.Clamp01(1 - (remainingDistance / distance)));
            remainingDistance -= gun.trailConfig.SimulationSpeed * Time.deltaTime;

            yield return null;
        }

        instance.transform.position = endPoint;

        if (hit.collider != null)
        {
            if (hit.transform.TryGetComponent(out IDamagable damagable))
            {
                DamagableType damagableType = damagable.damagableType;

                // Use a switch statement to handle different DamagableTypes
                switch (damagableType)
                {
                    case DamagableType.enemy:
                        // Handle damage for enemy type
                        damagable.TakeDamage(gun.damageConfig.GetDamage(distance));
                        break;
                    case DamagableType.player:
                        damagable.TakeDamage(gun.damageConfig.GetDamage(distance));
                        break;
                }
            }
        }

        yield return new WaitForSeconds(gun.trailConfig.Duration);
        yield return null;
        instance.emitting = false;
        instance.gameObject.SetActive(false);
        TrailPool.Release(instance);
    }


    public TrailRenderer CreateTrail()
    {
        GameObject instance = new GameObject("Bullet Trail");
        TrailRenderer trail = instance.AddComponent<TrailRenderer>();
        trail.colorGradient = gun.trailConfig.Color;
        trail.material = gun.trailConfig.Material;
        trail.widthCurve = gun.trailConfig.WidthCurve;
        trail.time = gun.trailConfig.Duration;
        trail.minVertexDistance = gun.trailConfig.MinVertexDistance;

        trail.emitting = false;
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        return trail;
    }

    public IEnumerator ResetCooldown()
    {
        yield return new WaitForSeconds(gun.damageConfig.reloadSpeed);
        currentAmmo = gun.damageConfig.MaxAmmo;
        WeaponFired?.Invoke(currentAmmo, this);
        isReloading = false;
    }
}
