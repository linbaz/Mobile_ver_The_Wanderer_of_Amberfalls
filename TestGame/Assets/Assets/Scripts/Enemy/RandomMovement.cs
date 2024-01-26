using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class RandomMovement2D : MonoBehaviour
{
    public Rigidbody2D rb;

    [Range(0, 100)] public float speed;
    [Range(1, 500)] public float walkRadius;
    public string wallTag = "Walls"; // Тег объектов, которые считаются стенами.

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

    private void SetNewDestination()
    {
        // Перед установкой новой точки назначения, проверяем, нет ли стены впереди.
        Vector2 newPosition = RandomNavMeshLocation();
        RaycastHit2D hit = Physics2D.Raycast(rb.position, newPosition - rb.position, Vector2.Distance(rb.position, newPosition), LayerMask.GetMask(wallTag));

        if (hit.collider == null)
        {
            randomDestination = newPosition;
        }
        else
        {
            // Если есть стена, попробуйте снова через некоторое время.
            Invoke("SetNewDestination", Random.Range(1f, 3f));
        }

        isMoving = true;
    }

    private Vector2 RandomNavMeshLocation()
    {
        Vector2 randomPosition = Random.insideUnitCircle * walkRadius;
        randomPosition += (Vector2)transform.position;

        return randomPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(wallTag))
        {
            // Если столкнулись с объектом, имеющим тег "Walls", то меняем точку назначения.
            SetNewDestination();
        }
    }
}
