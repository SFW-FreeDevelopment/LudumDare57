using UnityEngine;

public class PlayerLightController : MonoBehaviour
{
    [Range(0f, 1f)]
    public float lightCharge = 0.2f; // Current charge level (0 = no light, 1 = full glow)
    public float drainRate = 0.01f;

    void Update()
    {
        // Lose 0.01 per second (1/100 scale)
        lightCharge = Mathf.Clamp01(lightCharge - Time.deltaTime * drainRate);
    }
    
    // Optional: expose method to adjust light
    public void SetLightCharge(float value)
    {
        lightCharge = Mathf.Clamp01(value);
    }

    public float GetLightCharge()
    {
        return lightCharge;
    }
    
    public void AddCharge(float amount)
    {
        lightCharge = Mathf.Clamp01(lightCharge + amount);
    }
}
