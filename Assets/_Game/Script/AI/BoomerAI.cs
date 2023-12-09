using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerAI : BaseAI {

    private void Update() {
        if(CheckForPlayer() <= _aiConfig.ProximityToPlayer) {
            BoomOnPlayer();
        }
    }
    void BoomOnPlayer()
    {
        MoveAI();
        if (_distanceFromPlayer <= _aiConfig.DistanceForAbility) {
            Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, _aiConfig.ProximityToPlayer);

            foreach(var collider in colliders) {
                Rigidbody rb = collider.GetComponent<Rigidbody>();
                if (rb != null) {
                    if(rb.TryGetComponent(out IDamagable damagable)) {
                        
                        //Debug.Log("Damagable is taking damage " + damagable);
                        damagable.TakeDamage(_aiConfig.AIDamage/2);
                    }
                
                    rb.AddExplosionForce(20000, gameObject.transform.position, _aiConfig.ProximityToPlayer);
                }
            }

            TryGetComponent(out IDamagable boomerHealth);
            boomerHealth.TakeDamage(2000);

            
            // foreach (var collider in colliders)
            // {
            //     HealthBehavior health = collider.GetComponent<HealthBehavior>();
            //     Rigidbody rb = collider.GetComponent<Rigidbody>();
            //     if (rb != null)
            //     {
            //         if(health != null) {
            //             health.Damage(_aiConfig.AIDamage/2);
            //         }
            //         rb.AddExplosionForce(20000, gameObject.transform.position, _aiConfig.ProximityToPlayer);
            //     }
            // }
            // EnemyDead();
        }
    }
}
