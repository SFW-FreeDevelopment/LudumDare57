using UnityEngine;

public class NPCJellyfishDrift : MonoBehaviour
{
    public float horizontalSpeed = 0.2f;
    public float verticalAmplitude = 0.5f;
    public float verticalFrequency = 0.5f;

    private Vector3 startPosition;
    private float offset;

    void Start()
    {
        startPosition = transform.position;
        offset = Random.Range(0f, 2f * Mathf.PI); // desync vertical motion
    }

    void Update()
    {
        float x = Mathf.Sin(Time.time * horizontalSpeed + offset) * 0.5f;
        float y = Mathf.Sin(Time.time * verticalFrequency + offset) * verticalAmplitude;

        transform.position = startPosition + new Vector3(x, y, 0f);
        
        // Get current color
        Color c = GetComponent<SpriteRenderer>().color;

        // Oscillate alpha between min/max using sine wave
        float alpha = Mathf.Lerp(0.3f, 0.8f, (Mathf.Sin(Time.time * 0.5f + offset) + 1f) / 2f);
        c.a = alpha;

        // Apply it back
        GetComponent<SpriteRenderer>().color = c;
    }
}
