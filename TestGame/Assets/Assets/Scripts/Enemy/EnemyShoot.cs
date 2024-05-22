using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public float speed = 2f;
    public float entityDamage = 1f;
    public ToWeapon tw;
    private Rigidbody2D rb;
    public string shooterTag = "Enemy";

    // Добавляем переменную для отслеживания состояния выстрела
    private bool hasFired = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && !hasFired) // Проверяем состояние выстрела
        {
            Vector2 playerPosition = player.transform.position;
            Vector2 direction = (playerPosition - (Vector2)transform.position).normalized;
            Vector2 directionOther = (direction * 1000).normalized;

            rb.velocity = directionOther * speed;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // Устанавливаем флаг, что выстрел был произведен
            hasFired = true;
        }

        Invoke("DestroyTime", 4f);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Arrow"))
            return;

        if (other.gameObject.CompareTag(shooterTag))
            return;

        

        string entityTag = other.gameObject.tag;
        Health health = other.gameObject.GetComponent<Health>();

        if (health != null)
        {
            health.Reduce((int)entityDamage, health.currentHealth);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void DestroyTime()
    {
        Destroy(gameObject);
    }
}
