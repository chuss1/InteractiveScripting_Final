using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealable
{
    public float CurrentHealth { get; }
    public float MaxHealth { get; }
    public delegate void HealEvent(float damageAmount);
    public event HealEvent OnHeal;
    void Heal(float healAmount);
}
