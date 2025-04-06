using UnityEngine;

public class PlayerLightController : MonoBehaviour
{
    [Range(0f, 1f)]
    public float lightCharge = 1f; // Current charge level (0 = no light, 1 = full glow)

    // Optional: expose method to adjust light
    public void SetLightCharge(float value)
    {
        lightCharge = Mathf.Clamp01(value);
    }

    public float GetLightCharge()
    {
        return lightCharge;
    }
}
