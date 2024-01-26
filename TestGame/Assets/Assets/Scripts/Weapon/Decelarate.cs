using UnityEngine;

public class DecelerateOverTime : MonoBehaviour
{
    public float decelerationRate = 2.0f; // Скорость замедления (уменьшение скорости в секунду)

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (rb.velocity.magnitude > 0)
        {
            // Уменьшение скорости объекта с течением времени
            rb.velocity -= rb.velocity.normalized * decelerationRate * Time.deltaTime;

            // Проверка, чтобы скорость не стала отрицательной
            if (rb.velocity.magnitude < 0)
            {
                rb.velocity = Vector2.zero;
            }
        }
    }
}