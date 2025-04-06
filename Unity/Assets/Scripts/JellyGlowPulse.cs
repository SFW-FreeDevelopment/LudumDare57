using UnityEngine;

public class JellyGlowPulse : MonoBehaviour
{
    public ParticleSystem glowParticles;
    public float boostRate = 35f;
    public float normalRate = 7f;
    public float pulseDuration = 2f;

    private float pulseTimer = 0f;
    private ParticleSystem.EmissionModule emission;

    void Start()
    {
        if (glowParticles == null) glowParticles = GetComponent<ParticleSystem>();
        emission = glowParticles.emission;
        SetEmission(normalRate);
    }

    void Update()
    {
        if (pulseTimer > 0f)
        {
            pulseTimer -= Time.deltaTime;

            float t = 1f - (pulseTimer / pulseDuration);
            float curve = Mathf.Sin(t * Mathf.PI); // nice up/down curve from 0 → 1 → 0
            float currentRate = Mathf.Lerp(normalRate, boostRate, curve);

            SetEmission(currentRate);
        }
    }

    public void TriggerPulse()
    {
        pulseTimer = pulseDuration;
    }

    private void SetEmission(float rate)
    {
        var rateOverTime = emission.rateOverTime;
        rateOverTime.constant = rate;
        emission.rateOverTime = rateOverTime;
    }
}
