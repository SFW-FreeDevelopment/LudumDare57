using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DepthTrackerUI : MonoBehaviour
{
    public Transform playerTransform;
    public TextMeshProUGUI depthText;
    public Slider depthSlider;

    [Header("Settings")]
    public float maxDepthUnits = 1f;         // How many Unity units = full depth bar (e.g., 1f = 100m if using depthMultiplier)
    public float depthMultiplier = 100f;     // 1 Unity unit = X meters

    void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.GameOver) return;
        
        // Get how far below Y = 0 the player is
        float depthUnits = Mathf.Max(0f, -playerTransform.position.y);
        float depthMeters = depthUnits * depthMultiplier;

        // Clamp slider fill amount between 0–1
        float normalizedDepth = Mathf.Clamp01(depthUnits / maxDepthUnits);
        depthSlider.value = normalizedDepth;

        // Clamp displayed depth to slider max (so it stops at 100m if maxDepthUnits = 1)
        float maxMeters = maxDepthUnits * depthMultiplier;
        float clampedMeters = Mathf.Min(depthMeters, maxMeters);

        // Display as integer with "m"
        depthText.text = $"{Mathf.FloorToInt(clampedMeters)}m";
    }
}
