using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

// Клас для реалізації випадкового руху об'єкта 
public class RandomMovement2D : MonoBehaviour
{
    public Rigidbody2D rb;

    [Range(0, 100)] public float speed;
    [Range(1, 500)] public float walkRadius;
    public string wallTag = "Walls";

    private Vector2 randomDestination;
    private bool isMoving = false;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        randomDestination = RandomNavMeshLocation();
        isMoving = true;
    }

    public void Update()
    {
        if (isMoving)
        {
            Vector2 moveDirection = (randomDestination - rb.position).normalized;
            rb.velocity = moveDirection * speed;

            if (Vector2.Distance(rb.position, randomDestination) < 0.1f)
            {
                isMoving = false;
                rb.velocity = Vector2.zero;
                Invoke("SetNewDestination", Random.Range(1f, 3f));
            }
        }
    }

    // Встановлення нової випадкової точки призначення
    private void SetNewDestination()
    {
        Vector2 newPosition = RandomNavMeshLocation();
        RaycastHit2D hit = Physics2D.Raycast(rb.position, newPosition - rb.position, Vector2.Distance(rb.position, newPosition), LayerMask.GetMask(wallTag));

        // Перевірка, чи є перешкоди на шляху до нової точки, інакше встановлення нової точки
        if (hit.collider == null)
        {
            randomDestination = newPosition;
        }
        else
        {
            Invoke("SetNewDestination", Random.Range(1f, 3f));
        }

        isMoving = true;
    }

    // Випадкова точка в межах walkRadius
    private Vector2 RandomNavMeshLocation()
    {
        Vector2 randomPosition = Random.insideUnitCircle * walkRadius;
        randomPosition += (Vector2)transform.position;

        return randomPosition;
    }

    // Обробка зіткнення зі стіною
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(wallTag))
        {
            SetNewDestination();
        }
    }
}
