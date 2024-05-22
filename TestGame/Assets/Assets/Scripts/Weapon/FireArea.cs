using UnityEngine;

public class FireAreaCollider : MonoBehaviour
{
    // ”рон, наносимый врагам в области огн€
    public float damagePerSecond = 1f;

    // ѕри столкновении с другим коллайдером
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            return;

        if (other.gameObject.CompareTag("Arrow"))
            return;

        // ≈сли столкнулись с врагом
        if (other.CompareTag("Enemy"))
        {
            // Ќаносим урон врагу за каждую секунду нахождени€ в области огн€
            EntityStats enemy = other.GetComponent<EntityStats>();
            if (enemy != null)
            {
                enemy.GiveDamage(damagePerSecond * Time.deltaTime);
            }
        }
    }
}
