using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class LightMaskController : MonoBehaviour
{
    public Transform player;
    public Material maskMaterialSource;
    public PlayerLightController lightController;

    [Header("Base Light Settings")]
    public float minRadius = 0.1f;
    public float maxRadius = 0.3f;

    [Header("Pulse Settings")]
    public float pulseSpeed = 2f;
    public float pulseIntensity = 0.02f;

    private Material maskMaterialInstance;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;

        if (maskMaterialSource != null)
        {
            maskMaterialInstance = Instantiate(maskMaterialSource);
            GetComponent<RawImage>().material = maskMaterialInstance;
        }
        else
        {
            Debug.LogError("Mask material source not assigned.");
        }
    }

    void Update()
    {
        if (player == null || lightController == null || maskMaterialInstance == null || cam == null) return;

        // Convert player position to viewport space (0-1)
        Vector3 viewPos = cam.WorldToViewportPoint(player.position);
        Vector2 holeUV = new Vector2(viewPos.x, viewPos.y);
        maskMaterialInstance.SetVector("_HolePosition", holeUV);
        
        // Add subtle breathing pulse
        float clampedCharge = Mathf.Max(lightController.GetLightCharge(), 0.2f);
        float normalizedCharge = (clampedCharge - 0.2f) / 0.8f;
        float baseRadius = Mathf.Lerp(minRadius, maxRadius, normalizedCharge);
        float pulse = Mathf.Sin(Time.time * pulseSpeed) * pulseIntensity;
        float finalRadius = baseRadius + pulse;;

        maskMaterialInstance.SetFloat("_HoleRadius", finalRadius);
    }
}
