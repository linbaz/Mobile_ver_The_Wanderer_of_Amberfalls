using UnityEngine;

public class DecelerateOverTime : MonoBehaviour
{
    public float decelerationRate = 2.0f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (rb.velocity.magnitude > 0)
        {
            rb.velocity -= rb.velocity.normalized * decelerationRate * Time.deltaTime;

            if (rb.velocity.magnitude < 0)
            {
                rb.velocity = Vector2.zero;
            }
        }
    }
}