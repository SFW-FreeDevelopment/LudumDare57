using UnityEngine;

public class EnemyEelPatrol : MonoBehaviour
{
    public float speed = 2f;
    public float patrolDistance = 12f; // total distance it swims before turning around
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

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        direction = goingRight ? Vector3.right : Vector3.left;
        spriteRenderer.flipX = isReversedOrientation ? !goingRight : goingRight;

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
        float distanceFromStart = Vector3.Distance(new Vector3(transform.position.x, 0f, 0f), new Vector3(0f, 0f, 0f));
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
