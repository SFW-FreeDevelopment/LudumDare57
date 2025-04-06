using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GlowSliderSync : MonoBehaviour
{
    public Slider glowSlider;
    public TextMeshProUGUI glowText;
    public GameObject fullGlowIndicator; // Enable this when full
    public PlayerLightController lightController;

    [Header("Lerp Settings")]
    public float lerpSpeed = 5f;

    [Header("Text Animation")]
    public float pulseScale = 1.2f;
    public float pulseDuration = 0.2f;

    private float displayedValue = 0f;
    private int lastDisplayInt = -1;
    private Vector3 originalTextScale;
    private float pulseTimer = 0f;

    void Start()
    {
        if (glowText != null)
        {
            originalTextScale = glowText.transform.localScale;
        }

        if (lightController != null)
        {
            displayedValue = lightController.GetLightCharge() * 100f;
        }
    }

    void Update()
    {
        if (glowSlider == null || glowText == null || lightController == null)
            return;

        float targetValue = lightController.GetLightCharge() * 100f;

        // Lerp toward target number
        displayedValue = Mathf.Lerp(displayedValue, targetValue, Time.deltaTime * lerpSpeed);
        glowSlider.value = displayedValue / 100f;

        int displayInt = Mathf.RoundToInt(displayedValue);
        glowText.text = $"{displayInt}/100";

        // Trigger text pulse when whole number changes
        if (displayInt != lastDisplayInt)
        {
            lastDisplayInt = displayInt;
            pulseTimer = pulseDuration;
        }

        AnimateTextPulse();

        // Show "Full Glow" indicator if at 100
        if (fullGlowIndicator != null)
        {
            fullGlowIndicator.SetActive(displayInt >= 100);
        }
    }

    void AnimateTextPulse()
    {
        if (glowText == null) return;

        if (pulseTimer > 0f)
        {
            pulseTimer -= Time.deltaTime;
            float t = 1f - (pulseTimer / pulseDuration);
            float scale = Mathf.Lerp(pulseScale, 1f, t);
            glowText.transform.localScale = originalTextScale * scale;
        }
        else
        {
            glowText.transform.localScale = originalTextScale;
        }
    }
}
