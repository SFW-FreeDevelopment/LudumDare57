using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float fallSpeed = 1f;
    public float upwardBoost = 5f;
    public float levelWidth = 10f; // Total horizontal span of the level

    private Rigidbody2D rb;
    private Vector2 input;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f; // We'll control gravity manually
    }

    void Update()
    {
        // Get horizontal & vertical input
        input = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;

        // Spacebar gives a small upward boost
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, upwardBoost);
        }
    }

    void FixedUpdate()
    {
        // Continuous downward movement
        Vector2 downward = new Vector2(0, -fallSpeed);

        // Add player input for lateral control
        Vector2 movement = new Vector2(input.x * moveSpeed, rb.velocity.y);

        // Combine input with downward drift
        rb.velocity = new Vector2(movement.x, rb.velocity.y + downward.y * Time.fixedDeltaTime);
        
        // Clamp position X within level bounds
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -levelWidth / 2f, levelWidth / 2f);
        transform.position = pos;
    }
}