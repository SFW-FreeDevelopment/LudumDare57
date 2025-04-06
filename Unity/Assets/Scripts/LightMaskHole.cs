using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class LightMaskHole : MonoBehaviour
{
    public Material maskMaterial; // assign manually or pull from a shared controller
    public float radius = 0.2f;

    private Camera cam;
    private Material instanceMaterial;
    private SpriteRenderer sr;

    void Start()
    {
        cam = Camera.main;
        sr = GetComponent<SpriteRenderer>();

        if (maskMaterial != null)
        {
            instanceMaterial = Instantiate(maskMaterial);
            sr.material = instanceMaterial;
        }
    }

    void Update()
    {
        if (instanceMaterial == null || cam == null) return;

        Vector3 viewPos = cam.WorldToViewportPoint(transform.position);
        instanceMaterial.SetVector("_HolePosition", new Vector2(viewPos.x, viewPos.y));
        instanceMaterial.SetFloat("_HoleRadius", radius);
    }
}
