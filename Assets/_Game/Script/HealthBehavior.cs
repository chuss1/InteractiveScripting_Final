using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBehavior : MonoBehaviour, IDamagable, IHealable
{
    [SerializeField] private DamagableType _damagableType;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealth;

    public float CurrentHealth { get => _currentHealth; private set => _currentHealth = value; }
    public float MaxHealth { get => _maxHealth; private set => _maxHealth = value; }
    public DamagableType damagableType {get => _damagableType; private set => _damagableType = damagableType;}


    public event IDamagable.TakeDamageEvent OnTakeDamage;
    public event IDamagable.DeathEvent OnDeath;
    public event IHealable.HealEvent OnHeal;

    private void OnEnable() {
        CurrentHealth = MaxHealth;
    }

    private void UpdateHealth(float newHealth)
    {
        CurrentHealth = newHealth;

        if (OnTakeDamage != null)
            OnTakeDamage(0); // You can pass 0 as the damage amount or any appropriate value.

        if (CurrentHealth == 0 && OnDeath != null)
            OnDeath(transform.position);

        if (OnHeal != null)
            OnHeal(0); // You can pass 0 as the heal amount or any appropriate value.
    }

    public void TakeDamage(float damageAmount)
    {
        float damageTaken = Mathf.Clamp(damageAmount, 0, CurrentHealth);
        
        CurrentHealth -= damageTaken;

        if(damageTaken != 0) {
            //Debug.Log(gameObject.name + " Took " + damageTaken);
            OnTakeDamage?.Invoke(damageTaken);
        }

        if( CurrentHealth == 0 && damageTaken != 0) {
            OnDeath?.Invoke(transform.position);
        }
    }

    public void Heal(float healAmount)
    {
        float healTotal = Mathf.Clamp(healAmount, 0, CurrentHealth);

        CurrentHealth += healTotal;

        if(healTotal != 0)
        {
            //Debug.Log(gameObject.name + " Healed for " +  healTotal);
            OnHeal?.Invoke(healTotal);
        }
    }
}
