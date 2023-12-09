using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAI : MonoBehaviour {
    [SerializeField] protected AIConfigSO _aiConfig;
    protected Rigidbody _rb => GetComponent<Rigidbody>();
    protected Transform _playerTarget => GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    private GameManager gameManager => FindObjectOfType<GameManager>();
    private HealthBehavior _health => GetComponent<HealthBehavior>();
    protected float _distanceFromPlayer;
    protected bool isPlayerInRange = false;

    private void OnEnable() {
        _health.OnTakeDamage += EnemyGotHit;
        _health.OnDeath += EnemyDead;
    }


    private void OnDisable() {
        _health.OnTakeDamage -= EnemyGotHit;
        _health.OnDeath -= EnemyDead;
    }
    
    private void EnemyGotHit(float damageAmount) {}

    protected void EnemyDead(Vector3 position) {
        gameManager.RemoveEnemy(this);
        Destroy(gameObject);
    }

    protected float CheckForPlayer() {
        _distanceFromPlayer = Vector3.Distance(_playerTarget.position, transform.position);
        return _distanceFromPlayer;
    }

    protected void MoveAI() {
        transform.LookAt(_playerTarget);
        _rb.AddForce(transform.forward * _aiConfig.AISpeed * Time.deltaTime);
    }
}
