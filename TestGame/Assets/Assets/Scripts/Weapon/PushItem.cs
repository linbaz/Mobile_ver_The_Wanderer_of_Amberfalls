using UnityEngine;

public class PushOnCollision : MonoBehaviour
{
    public float pushForce = 10f; 

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direction = collision.contacts[0].point - (Vector2)transform.position;
                direction.Normalize();
                rb.AddForce(-direction * pushForce, ForceMode2D.Impulse);
            }
        }
    }
}