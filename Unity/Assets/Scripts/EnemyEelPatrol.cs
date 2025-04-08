using UnityEngine;

public class EnemyEelPatrol : MonoBehaviour
{
    public float speed = 2f;
    [Header("Patrol Bounds (world space)")]
    public float patrolMinX = -12f;
    public float patrolMaxX = 12f;
    public bool flipSpriteOnTurn = true;
    public bool goingRight = true;
    public bool isReversedOrientation = false;
    
    [Header("Animation Frames")]
    public Sprite[] eelSprites;           // Should be size 3
    public float animationFrameRate = 6f; // Frames per second
    
    private Vector3 direction = Vector3.right;
    private SpriteRenderer spriteRenderer;
    private int currentFrame = 0;
    private float animationTimer = 0f;
    private Vector3 startPos;
    private float patrolDistance;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Randomize starting X position between patrol bounds
        float startX = Random.Range(patrolMinX, patrolMaxX);
        transform.position = new Vector3(startX, transform.position.y, transform.position.z);
        startPos = transform.position;

        // Randomize direction
        goingRight = Random.value > 0.5f;
        direction = goingRight ? Vector3.right : Vector3.left;

        // Flip sprite if needed
        spriteRenderer.flipX = isReversedOrientation ? !goingRight : goingRight;

        // Calculate patrol distance based on how far to go in each direction
        patrolDistance = Mathf.Abs(patrolMaxX - patrolMinX) / 2f;

        // Randomize animation start frame
        if (eelSprites != null && eelSprites.Length > 0)
        {
            currentFrame = Random.Range(0, eelSprites.Length);
            spriteRenderer.sprite = eelSprites[currentFrame];
        }
    }

    void Update()
    {
        // Move horizontally only
        transform.position += direction * speed * Time.deltaTime;

        // Check distance from start
        float distanceFromStart = Mathf.Abs(transform.position.x - startPos.x);
        if (distanceFromStart >= patrolDistance)
        {
            // Turn around
            direction *= -1;

            if (flipSpriteOnTurn && spriteRenderer != null)
            {
                spriteRenderer.flipX = isReversedOrientation ? direction.x < 0 : direction.x >= 0;
            }
        }
        
        // Animate wiggly eel
        AnimateEel();
    }
    
    void AnimateEel()
    {
        if (eelSprites == null || eelSprites.Length < 3 || spriteRenderer == null)
            return;

        animationTimer += Time.deltaTime;
        float frameDuration = 1f / animationFrameRate;

        if (animationTimer >= frameDuration)
        {
            animationTimer -= frameDuration;
            currentFrame = (currentFrame + 1) % eelSprites.Length;
            spriteRenderer.sprite = eelSprites[currentFrame];
        }
    }
}
