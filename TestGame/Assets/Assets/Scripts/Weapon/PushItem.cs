using UnityEngine;

public class PushOnCollision : MonoBehaviour
{
    public float pushForce = 10f; // Сила, с которой объект будет сдвигаться

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Можете настроить условия в зависимости от вашей игры
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Примените силу к объекту в направлении столкновения
                Vector2 direction = collision.contacts[0].point - (Vector2)transform.position;
                direction.Normalize();
                rb.AddForce(-direction * pushForce, ForceMode2D.Impulse);
            }
        }
    }
}