using UnityEngine;

public class LightMaskController : MonoBehaviour
{
    public Transform player;
    public Material maskMaterial;
    public float radius = 0.2f;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (player == null || maskMaterial == null || cam == null) return;

        // This gives us a value in 0–1 viewport space (safe for UI shader)
        Vector3 viewPos = cam.WorldToViewportPoint(player.position);

        // Clamp just in case (should always be safe)
        viewPos.x = Mathf.Clamp01(viewPos.x);
        viewPos.y = Mathf.Clamp01(viewPos.y);

        // Send to shader
        maskMaterial.SetVector("_HolePosition", new Vector2(viewPos.x, viewPos.y));
        maskMaterial.SetFloat("_HoleRadius", radius);
    }

    public void SetRadius(float newRadius)
    {
        radius = newRadius;
    }
}
