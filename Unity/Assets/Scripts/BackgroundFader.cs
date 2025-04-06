using UnityEngine;

public class BackgroundFaderByDepth : MonoBehaviour
{
    public Transform player;
    public Color startColor = new Color32(0x00, 0x3f, 0x5c, 255); // #003f5c
    public Color targetColor = new Color32(0x00, 0x08, 0x14, 255); // or a deeper blue if desired
    public float maxFadeDistance = 100f; // How far the player falls before fully faded
    public SpriteRenderer ambientTexture;
    
    [Header("Pulse Settings")]
    public float pulseSpeed = 0.5f; // slower is more subtle
    public float pulseIntensity = 0.05f; // how much to brighten/dim (e.g. 0.05 = ±5%)

    private Camera cam;
    private float startY;

    void Start()
    {
        cam = Camera.main;
        if (player == null) player = GameObject.FindWithTag("Player")?.transform;

        if (player != null)
        {
            startY = player.position.y;
        }

        cam.backgroundColor = startColor;
        ambientTexture.color = startColor;
    }

    void Update()
    {
        if (player == null) return;

        // Depth-based fade
        float distanceFallen = startY - player.position.y;
        float t = Mathf.Clamp01(distanceFallen / maxFadeDistance);
        Color baseColor = Color.Lerp(startColor, targetColor, t);

        // Brightness pulse using sine wave
        float pulse = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseIntensity;

        // Apply pulse to color
        Color pulsedColor = baseColor * pulse;
        pulsedColor.a = 1f; // force alpha back to 1

        // Clamp to prevent values >1
        pulsedColor.r = Mathf.Clamp01(pulsedColor.r);
        pulsedColor.g = Mathf.Clamp01(pulsedColor.g);
        pulsedColor.b = Mathf.Clamp01(pulsedColor.b);

        cam.backgroundColor = pulsedColor;
        ambientTexture.color = pulsedColor;
    }
}
