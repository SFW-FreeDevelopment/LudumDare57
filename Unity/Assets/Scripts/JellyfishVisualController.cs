using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class JellyfishVisualController : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite baseSprite;         // Idle frame 1
    public Sprite idleSprite;         // Idle frame 2
    public Sprite idleBlinkSprite;  // Blink version of idleSprite2
    public Sprite moveSprite1;        // Swimming frame 1
    public Sprite moveSprite2;        // Swimming frame 2
    public Sprite moveBlinkSprite;    // Rare blink while swimming
    public Sprite jumpSprite;         // Jump pose

    [Header("Animation Settings")]
    public float movementThreshold = 0.05f;
    public float idleSwapRate = 0.5f;
    public float moveSwapRate = 0.4f;
    public float jumpDisplayTime = 0.2f;
    
    [Header("Blink Settings")]
    public float blinkChance = 0.2f;
    public float blinkHoldTime = 0.25f;

    private bool showingIdle = false;
    private bool isBlinking = false;
    private float blinkTimer = 0f;

    private SpriteRenderer sr;
    private Vector3 lastPosition;
    private float idleTimer;
    private float moveTimer;
    private bool showingIdleAlt = false;
    private bool showingMoveAlt = false;
    private float jumpTimer;
    private Transform tr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        tr = transform;
        lastPosition = tr.position;
    }

    void Update()
    {
        Vector3 velocity = (tr.position - lastPosition) / Time.deltaTime;
        lastPosition = tr.position;

        float horizontalSpeed = Mathf.Abs(velocity.x);
        bool isMoving = horizontalSpeed > movementThreshold;

        // Flip sprite based on movement direction
        if (Mathf.Abs(velocity.x) > 0.01f)
        {
            sr.flipX = velocity.x < 0f;
        }

        // Trigger jump (customize this to your actual jump logic)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpTimer = jumpDisplayTime;
        }

        // Handle sprite display
        if (jumpTimer > 0f)
        {
            sr.sprite = jumpSprite;
            jumpTimer -= Time.deltaTime;
            tr.localScale = Vector3.one;
        }
        else if (isMoving)
        {
            HandleMoveAnimation();
        }
        else
        {
            HandleIdleAnimation();
        }
    }

    void HandleIdleAnimation()
    {
        idleTimer += Time.deltaTime;

        if (idleTimer >= idleSwapRate)
        {
            idleTimer = 0f;
            showingIdle = !showingIdle;

            // Only possibly blink when showing idleSprite
            isBlinking = showingIdle && Random.value < 0.2f;
        }

        if (showingIdle)
        {
            sr.sprite = isBlinking && idleBlinkSprite != null ? idleBlinkSprite : idleSprite;
        }
        else
        {
            sr.sprite = baseSprite;
        }

        tr.localScale = Vector3.one;
    }

    void HandleMoveAnimation()
    {
        if (isBlinking)
        {
            blinkTimer -= Time.deltaTime;
            if (blinkTimer <= 0f)
            {
                isBlinking = false;
            }
            sr.sprite = moveBlinkSprite;
            return;
        }

        moveTimer += Time.deltaTime;

        if (moveTimer >= moveSwapRate)
        {
            moveTimer = 0f;
            showingMoveAlt = !showingMoveAlt;

            // Only trigger blink when switching to alt frame
            if (showingMoveAlt && moveBlinkSprite != null)
            {
                float chance = Random.Range(0f, 1f);
                if (chance < blinkChance)
                {
                    isBlinking = true;
                    blinkTimer = blinkHoldTime;
                    sr.sprite = moveBlinkSprite;
                    return;
                }
            }
        }

        sr.sprite = showingMoveAlt ? moveSprite2 : moveSprite1;
    }
}
