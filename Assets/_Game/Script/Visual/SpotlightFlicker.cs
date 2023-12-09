using UnityEngine;

public class SpotlightFlicker : MonoBehaviour
{
    public float minIntensity = 0.5f; // Minimum intensity of the spotlight
    public float maxIntensity = 1.0f; // Maximum intensity of the spotlight
    public float flickerInterval = 2.0f; // Time interval for flickering

    private Light spotlight;
    private float nextFlickerTime;

    void Start()
    {
        spotlight = GetComponentInChildren<Light>();
        nextFlickerTime = Time.time + Random.Range(0, flickerInterval);
    }

    void Update()
    {
        if (Time.time >= nextFlickerTime)
        {
            // Randomly set the intensity within the specified range
            spotlight.intensity = Random.Range(minIntensity, maxIntensity);

            // Schedule the next flicker
            nextFlickerTime = Time.time + flickerInterval;
        }
    }
}
