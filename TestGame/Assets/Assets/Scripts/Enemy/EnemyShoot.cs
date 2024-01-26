using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public float speed = 2f;
    public float entityDamage = 1f;
    public ToWeapon tw;
    private Rigidbody2D rb;

    // Assign a unique tag to the shooting enemy, e.g., "EnemyShooter"
    public string shooterTag = "Enemy";

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector2 playerPosition = player.transform.position;
            Vector2 direction = (playerPosition - (Vector2)transform.position).normalized;
            Vector2 directionOther = (direction * 1000).normalized;
            rb.velocity = directionOther * speed;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        Invoke("DestroyTime", 4f);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        // Check if the collided object has the same tag as the shooter
        if (other.gameObject.CompareTag(shooterTag))
        {
            return;
        }

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
