using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class JellyfishVisualController : MonoBehaviour
{
    public Sprite idleSprite;
    public Sprite moveSprite;

    public float movementThreshold = 0.05f;

    private SpriteRenderer sr;
    private Vector3 lastPosition;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        lastPosition = transform.position;
    }

    void Update()
    {
        Vector3 velocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;

        float horizontalSpeed = Mathf.Abs(velocity.x);

        if (horizontalSpeed > movementThreshold)
        {
            sr.sprite = moveSprite;
        }
        else
        {
            sr.sprite = idleSprite;
        }

        // Flip based on horizontal movement
        if (Mathf.Abs(velocity.x) > 0.01f)
        {
            sr.flipX = velocity.x < 0f;
        }
    }
}
