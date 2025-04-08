using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float fallSpeed = 1f;
    public float boostForce = 5f;
    public float verticalControlSpeed = 2f;
    public float levelWidth = 10f;
    public float maxFallSpeed = -5f;
    public float maxRiseSpeed = 5f;
    public float boostXMultiplier = 1.5f;

    private Rigidbody2D rb;
    private Vector2 input;
    private int verticalIntent = 0; // 1 = up, -1 = down, 0 = neutral
    private int horizontalIntent = 0; // 1 = right, -1 = left, 0 = none

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }

    void Update()
    {
        // Update horizontal input (remember last non-zero direction)
        float rawX = Input.GetAxisRaw("Horizontal");
        input = new Vector2(rawX, 0f).normalized;

        if (rawX > 0) horizontalIntent = 1;
        else if (rawX < 0) horizontalIntent = -1;

        // Update vertical intent
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            verticalIntent = 1;
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            verticalIntent = -1;

        // Boost in the direction of intent
        if (Input.GetKeyDown(KeyCode.Space) && verticalIntent != 0)
        {
            float boostY = verticalIntent * boostForce;
            float boostX = horizontalIntent * moveSpeed * boostXMultiplier;

            rb.velocity = new Vector2(boostX, boostY);
        }
    }

    void FixedUpdate()
    {
        // Apply gravity-like downward force
        float newY = rb.velocity.y + (-fallSpeed * Time.fixedDeltaTime);

        // Smoothly add vertical intent if holding W or S
        newY += verticalIntent * verticalControlSpeed * Time.fixedDeltaTime;

        // Clamp vertical speed
        newY = Mathf.Clamp(newY, maxFallSpeed, maxRiseSpeed);

        // Apply movement
        float moveX = input.x * moveSpeed;
        rb.velocity = new Vector2(moveX, newY);

        // Clamp horizontal position
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -levelWidth / 2f, levelWidth / 2f);
        transform.position = pos;
    }
}
