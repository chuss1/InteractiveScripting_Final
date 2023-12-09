using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the entered object has the IDamagable interface
        IDamagable damagable = other.GetComponent<IDamagable>();

        // If the entered object has the IDamagable interface, apply damage
        if (damagable != null)
        {
            damagable.TakeDamage(10000f);
        }
    }
}
