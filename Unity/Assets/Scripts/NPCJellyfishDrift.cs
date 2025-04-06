using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class NPCJellyfishDrift : MonoBehaviour
{
    [Header("Movement")]
    public float horizontalSpeed = 0.2f;
    public float verticalAmplitude = 0.5f;
    public float verticalFrequency = 0.5f;

    [Header("Idle Animation")]
    public Sprite baseSprite;
    public Sprite idleSprite;
    public float idleSwapRate = 0.5f;

    [Header("Pulse Settings")]
    public float pulseSpeed = 2f;
    public float pulseIntensity = 0.03f;

    private Vector3 startPosition;
    private float movementOffset;
    private float alphaOffset;
    private float scaleOffset;

    private SpriteRenderer sr;
    private float idleTimer = 0f;
    private float randomizedIdleSwapRate;
    private bool showingAltIdleFrame = false;

    private Transform tr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        tr = transform;
        startPosition = tr.position;

        movementOffset = Random.Range(0f, 2f * Mathf.PI);
        alphaOffset = Random.Range(0f, 2f * Mathf.PI);
        scaleOffset = Random.Range(0f, 2f * Mathf.PI);

        randomizedIdleSwapRate = idleSwapRate * Random.Range(0.8f, 1.2f);
        idleTimer = Random.Range(0f, randomizedIdleSwapRate);
    }

    void Update()
    {
        float time = Time.time;

        // Movement
        float x = Mathf.Sin(time * horizontalSpeed + movementOffset) * 0.5f;
        float y = Mathf.Sin(time * verticalFrequency + movementOffset) * verticalAmplitude;
        tr.position = startPosition + new Vector3(x, y, 0f);

        // Alpha Pulse
        Color c = sr.color;
        float alpha = Mathf.Lerp(0.3f, 0.8f, (Mathf.Sin(time * 0.5f + alphaOffset) + 1f) / 2f);
        c.a = alpha;
        sr.color = c;

        // Idle Animation
        idleTimer += Time.deltaTime;
        if (idleTimer >= randomizedIdleSwapRate)
        {
            idleTimer = 0f;
            showingAltIdleFrame = !showingAltIdleFrame;
        }

        sr.sprite = showingAltIdleFrame ? idleSprite : baseSprite;

        // Scale Pulse (squish/stretch)
        float scaleX = 1f + Mathf.Sin(time * pulseSpeed + scaleOffset) * pulseIntensity;
        float scaleY = 1f - Mathf.Sin(time * pulseSpeed + scaleOffset) * pulseIntensity;
        tr.localScale = new Vector3(scaleX, scaleY, 1f);
    }
}
