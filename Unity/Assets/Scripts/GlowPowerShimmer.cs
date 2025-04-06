using UnityEngine;
using UnityEngine.UI;

public class GlowPowerShimmer : MonoBehaviour
{
    public Image fillImage;

    [Header("Shimmer Settings")]
    public Color baseColor = new Color(1f, 0.812f, 0f, 0.6f);
    public float hueRange = 0.05f;        // How much the hue shifts
    public float valueRange = 0.1f;       // Brightness change
    public float shimmerSpeed = 2f;       // How fast it cycles

    private float shimmerOffset;

    void Start()
    {
        if (fillImage == null)
            fillImage = GetComponent<Image>();

        shimmerOffset = Random.Range(0f, 100f); // Desync shimmer across UI elements if needed
    }

    void Update()
    {
        float t = (Mathf.Sin(Time.time * shimmerSpeed + shimmerOffset) + 1f) / 2f;

        // Convert base color to HSV
        Color.RGBToHSV(baseColor, out float h, out float s, out float v);

        // Modulate hue and brightness slightly
        float shimmerHue = h + Random.Range(-hueRange, hueRange);
        float shimmerValue = v + Mathf.Lerp(-valueRange, valueRange, t);

        Color shimmerColor = Color.HSVToRGB(shimmerHue, s, shimmerValue);
        shimmerColor.a = baseColor.a;

        fillImage.color = shimmerColor;
    }
}
