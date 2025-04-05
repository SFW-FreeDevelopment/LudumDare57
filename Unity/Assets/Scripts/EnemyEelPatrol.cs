using UnityEngine;

public class EnemyEelPatrol : MonoBehaviour
{
    public float speed = 2f;
    public float patrolDistance = 12f; // total distance it swims before turning around
    public bool flipSpriteOnTurn = true;

    private Vector3 startPosition;
    private Vector3 direction = Vector3.right;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        startPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Move horizontally only
        transform.position += direction * speed * Time.deltaTime;

        // Check distance from start
        float distanceFromStart = Vector3.Distance(new Vector3(transform.position.x, 0f, 0f), new Vector3(startPosition.x, 0f, 0f));
        if (distanceFromStart >= patrolDistance)
        {
            // Turn around
            direction *= -1;

            if (flipSpriteOnTurn && spriteRenderer != null)
            {
                spriteRenderer.flipX = direction.x >= 0;
            }
        }
    }
}
