using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{
    [SerializeField] private HealthBehavior playerHealth;
    [SerializeField] private Slider healthSlider;

    private void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthBehavior>();
        healthSlider = GetComponent<Slider>();

        if (playerHealth != null)
        {
            healthSlider.maxValue = playerHealth.MaxHealth;
            healthSlider.value = playerHealth.CurrentHealth;

            // Listen to the OnTakeDamage and OnHeal events
            playerHealth.OnTakeDamage += UpdateSlider;
            playerHealth.OnHeal += UpdateSlider;
        }
    }

    private void UpdateSlider(float amount)
    {
        // Update the slider's value when the events are invoked
        if (playerHealth != null)
        {
            healthSlider.value = playerHealth.CurrentHealth;
        }
    }
}
