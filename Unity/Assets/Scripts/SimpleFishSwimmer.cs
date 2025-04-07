using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SimpleFishSwimmer : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 2f;
    public float verticalAmplitude = 0.5f;
    public float verticalLerpSpeed = 1f;

    [Header("Patrol Bounds (world space)")]
    public float patrolMinX = -12f;
    public float patrolMaxX = 12f;

    [Header("Animation")]
    public Sprite[] animationFrames;
    public float frameRate = 4f;

    [Header("Facing & Light")]
    public bool spriteFacesLeftByDefault = true;
    public Transform lightHoleTransform;

    private SpriteRenderer sr;
    private Vector3 startPos;
    private float direction = 1f;
    private float animTimer = 0f;
    private int currentFrame = 0;
    private float verticalTargetOffset = 0f;
    private float verticalCurrentOffset = 0f;
    private Vector3 initialLightHoleLocalPosition;
    private Vector3 initialLightHoleLocalScale;
    private BoxCollider2D boxCollider;
    private Vector2 initialColliderOffset;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        startPos = new Vector3(0f, transform.position.y, transform.position.z);

        if (animationFrames.Length > 0)
            sr.sprite = animationFrames[0];

        if (lightHoleTransform != null)
        {
            initialLightHoleLocalPosition = lightHoleTransform.localPosition;
            initialLightHoleLocalScale = lightHoleTransform.localScale;
        }

        boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            initialColliderOffset = boxCollider.offset;
        }
    }

    void Update()
    {
        // Move horizontally
        float move = direction * speed * Time.deltaTime;
        transform.position += new Vector3(move, 0f, 0f);

        // Flip sprite only
        bool facingLeft = direction < 0;
        sr.flipX = facingLeft ^ spriteFacesLeftByDefault;

        // Flip collider offset
        if (boxCollider != null)
        {
            boxCollider.offset = new Vector2(
                facingLeft ? -Mathf.Abs(initialColliderOffset.x) : Mathf.Abs(initialColliderOffset.x),
                initialColliderOffset.y
            );
        }

        // Patrol based on world X bounds
        float currentX = transform.position.x;
        if (direction < 0f && currentX <= patrolMinX)
        {
            direction = 1f;
        }
        else if (direction > 0f && currentX >= patrolMaxX)
        {
            direction = -1f;
        }

        // Gentle vertical offset
        if (Mathf.Abs(verticalCurrentOffset - verticalTargetOffset) < 0.05f)
        {
            verticalTargetOffset = Random.Range(-verticalAmplitude, verticalAmplitude);
        }

        verticalCurrentOffset = Mathf.Lerp(verticalCurrentOffset, verticalTargetOffset, Time.deltaTime * verticalLerpSpeed);
        transform.position = new Vector3(transform.position.x, startPos.y + verticalCurrentOffset, transform.position.z);

        // Animate
        if (animationFrames.Length > 1)
        {
            animTimer += Time.deltaTime;
            if (animTimer >= 1f / frameRate)
            {
                animTimer = 0f;
                currentFrame = (currentFrame + 1) % animationFrames.Length;
                sr.sprite = animationFrames[currentFrame];
            }
        }

        // Mirror the light hole
        if (lightHoleTransform != null)
        {
            lightHoleTransform.localPosition = new Vector3(
                facingLeft ? -initialLightHoleLocalPosition.x : initialLightHoleLocalPosition.x,
                initialLightHoleLocalPosition.y,
                initialLightHoleLocalPosition.z
            );
        }
    }
}
