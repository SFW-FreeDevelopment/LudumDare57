using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ScrollingTexture : MonoBehaviour
{
    public Vector2 scrollSpeed = new Vector2(0.01f, 0.02f); // x = horizontal, y = vertical

    private SpriteRenderer sr;
    private Material mat;
    private Vector2 currentOffset = Vector2.zero;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        mat = sr.material;

        // Ensure it's using a material instance, not the shared one
        sr.material = new Material(mat);
        mat = sr.material;
    }

    void Update()
    {
        currentOffset += scrollSpeed * Time.deltaTime;
        mat.mainTextureOffset = currentOffset;
    }
}