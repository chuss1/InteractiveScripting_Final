using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IDamagable))]
public class SpawnParticleSystemOnDeath : MonoBehaviour {
    [SerializeField] private ParticleSystem DeathParticles;
    public IDamagable Damagable;

    private void Awake() {
        Damagable = GetComponent<IDamagable>();
    }

    private void OnEnable() {
        Damagable.OnDeath += Damagable_OnDeath;
    }

    private void OnDisable() {
        Damagable.OnDeath -= Damagable_OnDeath;
    }

    private void Damagable_OnDeath(Vector3 position)
    {
        GameObject particles = Instantiate(DeathParticles, position, Quaternion.identity).gameObject;

        Destroy(particles, 1f);
    }
}
