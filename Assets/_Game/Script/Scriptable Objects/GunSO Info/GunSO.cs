using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Gun", menuName = "Guns/Gun", order = 0)]
public class GunSO : ScriptableObject {
    public GunType Type;
    public string Name;
    public GameObject ModelPrefab;
    public Vector3 SpawnPoint;
    public Vector3 SpawnRotation;
    public int currentAmmo;
    public AudioSource gunShotSFX;
    public Animator animator;
    public Action<int, GunSO> WeaponFired;

    public DamageConfigSO damageConfig;
    public ShootConfigSO shootConfig;
    public TrailConfigSO trailConfig;

    private MonoBehaviour ActiveMonoBehaviour;
    public GameObject Model;
    private float LastShootTime;
    private ParticleSystem ShootSystem;
    private ObjectPool<TrailRenderer> TrailPool;
    private bool isReloading;
    public bool isSpawned = false;

    public void Spawn(Transform parent, MonoBehaviour activeMonoBehaviour) {
        Debug.Log("Spawning " + name);
        this.ActiveMonoBehaviour = activeMonoBehaviour;
        currentAmmo = damageConfig.MaxAmmo;
        isReloading = false;
        LastShootTime = 0;
        TrailPool = new ObjectPool<TrailRenderer>(CreateTrail);

        Model = Instantiate(ModelPrefab);
        Model.transform.SetParent(parent, false);
        Model.transform.localPosition = SpawnPoint;
        Model.transform.localRotation = Quaternion.Euler(SpawnRotation);
        gunShotSFX = Model.GetComponent<AudioSource>();
        animator = Model.GetComponent<Animator>();
        isSpawned = true;

        ShootSystem = Model.GetComponentInChildren<ParticleSystem>();

        WeaponFired?.Invoke(currentAmmo, this);
    }

    public void Shoot() {
        if(Time.time > shootConfig.FireRate + LastShootTime && !isReloading) {
            Debug.Log(name + " is shooting");
            LastShootTime = Time.time;
            ShootSystem.Play();
            currentAmmo--;
            WeaponFired?.Invoke(currentAmmo, this);
            gunShotSFX.Play();
            animator.SetTrigger("GunShoot");
            if(currentAmmo == 0) isReloading = true;

            Vector3 shootDirection = ShootSystem.transform.forward + new Vector3(Random.Range(-shootConfig.Spread.x, shootConfig.Spread.x), 
                                                                                 Random.Range(-shootConfig.Spread.y, shootConfig.Spread.y), 
                                                                                 Random.Range(-shootConfig.Spread.z, shootConfig.Spread.z));

            shootDirection.Normalize();

            if(Physics.Raycast(ShootSystem.transform.position, shootDirection, out RaycastHit hit, float.MaxValue, shootConfig.HitMask)) {
                ActiveMonoBehaviour.StartCoroutine(PlayTrail(ShootSystem.transform.position, hit.point, hit));
            } else {
                ActiveMonoBehaviour.StartCoroutine(PlayTrail(ShootSystem.transform.position, ShootSystem.transform.position + (shootDirection * trailConfig.MissDistance), new RaycastHit()));
            }
            if(currentAmmo == 0) {
                ActiveMonoBehaviour.StartCoroutine(ResetCooldown());
            }
        }
    }

    private IEnumerator PlayTrail(Vector3 startPoint, Vector3 endPoint, RaycastHit hit) {
        TrailRenderer instance = TrailPool.Get();
        instance.gameObject.SetActive(true);
        instance.transform.position = startPoint;
        yield return null; // avoid position carry-over from last frame if reused

        instance.emitting = true;

        float distance = Vector3.Distance(startPoint, endPoint);
        float remainingDistance = distance;
        while(remainingDistance > 0) {
            instance.transform.position = Vector3.Lerp(startPoint, endPoint, Mathf.Clamp01(1 - (remainingDistance / distance)));
            remainingDistance -= trailConfig.SimulationSpeed * Time.deltaTime;
            
            yield return null;
        }

        instance.transform.position = endPoint;

        if(hit.collider != null) {
            if(hit.transform.TryGetComponent(out IDamagable damagable)) {
                damagable.TakeDamage(damageConfig.GetDamage(distance));
                Debug.Log("Hit " + damagable);
            }
        }

        yield return new WaitForSeconds(trailConfig.Duration);
        yield return null;
        instance.emitting = false;
        instance.gameObject.SetActive(false);
        TrailPool.Release(instance);
    }

    private TrailRenderer CreateTrail() {
        GameObject instance = new GameObject("Bullet Trail");
        TrailRenderer trail = instance.AddComponent<TrailRenderer>();
        trail.colorGradient = trailConfig.Color;
        trail.material = trailConfig.Material;
        trail.widthCurve = trailConfig.WidthCurve;
        trail.time = trailConfig.Duration;
        trail.minVertexDistance = trailConfig.MinVertexDistance;

        trail.emitting = false;
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        return trail;
    }

    private IEnumerator ResetCooldown()
    {
        Debug.Log("Reloading");
        yield return new WaitForSeconds(damageConfig.reloadSpeed);
        currentAmmo = damageConfig.MaxAmmo;
        WeaponFired?.Invoke(currentAmmo, this);
        isReloading = false;
    }
}
