using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class RangedTest : MonoBehaviour
{
    [SerializeField] float chaseRadius = 10f; // Радиус преследования игрока
    [SerializeField] float walkRadius = 5f;   // Радиус случайного блуждания
    [SerializeField] float moveSpeed = 3f; // Параметр скорости
    [SerializeField] string wallTag = "Walls"; // Тег объектов, которые считаются стенами.

    private Transform player;                  // Ссылка на игрока
    private NavMeshAgent agent;                // Компонент NavMeshAgent
    private Vector3 randomDestination;         // Случайная точка назначения
    private bool isShooting = false;            // Флаг для отслеживания состояния преследования

    public GameObject bullet;
    private float shootCooldown;
    public float startShootCooldown;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        shootCooldown = startShootCooldown;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.transform.rotation = Quaternion.Euler(0, 0, 0);

        // Устанавливаем параметр скорости
        agent.speed = moveSpeed;

        // Находим игрока по тегу "Player"
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Запускаем случайное блуждание
        SetNewRandomDestination();
        StartCoroutine(RandomDestinationRoutine());
    }

    private void Update()
    {

        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Если расстояние до игрока меньше радиуса преследования, преследуем игрока
            if (distanceToPlayer <= chaseRadius)
            {
                isShooting = true;
                SetNewRandomDestination();
            }
            else if (isShooting)
            {
                // Если преследование было активировано, но игрок вышел из радиуса, перейдем в случайное блуждание
                isShooting = false;
                SetNewRandomDestination();
            }
            else if (!isShooting && Vector3.Distance(transform.position, randomDestination) < 0.3f)
            {
                // Если не преследуем игрока и достигли случайной точки назначения, устанавливаем новую
                SetNewRandomDestination();
            }

            if (isShooting)
            {
                Vector2 direction = new Vector2(player.position.x - transform.position.x, player.position.y - transform.position.y);

                transform.up = direction;

                if (shootCooldown <= 0)
                {
                    Instantiate(bullet, transform.position, transform.rotation);
                    shootCooldown = startShootCooldown;
                }
                else
                {
                    shootCooldown -= Time.deltaTime;
                }

            }
        }
        else
        {
            isShooting = false;
        }

    }


    private void SetNewRandomDestination()
    {
        // Перед установкой новой точки назначения, проверяем, нет ли стены впереди.
        Vector3 newPosition = RandomNavMeshLocation();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, newPosition - transform.position, out hit, Vector3.Distance(transform.position, newPosition)) &&
            hit.collider.CompareTag(wallTag))
        {
            // Если есть стена, попробуйте снова через некоторое время.
            Invoke("SetNewRandomDestination", Random.Range(1f, 3f));
        }
        else
        {
            randomDestination = newPosition;
            agent.SetDestination(randomDestination);
        }
    }

   
    private Vector3 RandomNavMeshLocation()
    {
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);

        return hit.position;
    }

    private IEnumerator RandomDestinationRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            SetNewRandomDestination();        
        }
    }
   
}
