using UnityEngine;

public class LightHolePulse : MonoBehaviour
{
    public float pulseAmplitude = 0.05f;
    public float pulseSpeed = 2f;

    private Vector3 baseScale;

    void Start()
    {
        baseScale = transform.localScale;
    }

    void Update()
    {
        float pulse = Mathf.Sin(Time.time * pulseSpeed) * pulseAmplitude;
        float scale = 1f + pulse;
        transform.localScale = baseScale * scale;
    }
}