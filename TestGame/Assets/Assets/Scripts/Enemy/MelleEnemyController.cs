using UnityEngine.AI;
using UnityEngine;
using System.Collections;

// Клас, що реалізує поведінку ворожого об'єкта
public class test : MonoBehaviour
{
    [SerializeField] private float entityDamage = 2f;
    [SerializeField] private float damageInterval = 2f;
    [SerializeField] float chaseRadius = 10f;
    [SerializeField] float walkRadius = 5f;
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] string wallTag = "Walls";

    private bool isCooldown = false;
    private float fleeCooldownDuration = 2f;
    private bool hasNewOppositePoint = false;
    private bool isFleeing = false;
    private float lastDamageTime;
    private Transform player;
    private NavMeshAgent agent;
    private Vector3 randomDestination;
    private bool isChasing = false;
    private bool shouldFollowPlayer = false;

    private void Start()
    {
        // Отримання посилання на гравця та ініціалізація компонента NavMeshAgent
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.transform.rotation = Quaternion.Euler(0, 0, 0);

        agent.speed = moveSpeed;

        // Встановлення нової випадкової точки призначення та запуск рутиною для зміни її кожні 2 секунди
        SetNewRandomDestination();
        StartCoroutine(RandomDestinationRoutine());
    }

    private void Update()
    {
        // Перевірка наявності гравця
        if (player != null)
        {
            // Перевірка, чи об'єкт не виконує підрахунок шляху та чи відстань менше 0,5f
            if (!agent.pathPending && agent.remainingDistance < 0.5f && !isChasing)
            {
                isChasing = true;
                shouldFollowPlayer = true;

                if (!isFleeing)
                {
                    agent.SetDestination(player.position);
                }
            }

            // Переміщення за гравцем, якщо активовано слідування
            if (shouldFollowPlayer)
            {
                agent.SetDestination(player.position);
            }

            // Відміна слідування та встановлення нової протилежної точки, якщо ворог виходить за радіус переслідування
            if (isChasing && Vector3.Distance(transform.position, player.position) > chaseRadius)
            {
                isChasing = false;
                shouldFollowPlayer = false;
                SetNewOppositePoint();
                agent.SetDestination(randomDestination);

                hasNewOppositePoint = false;
            }
        }
        else
        {
            isChasing = false;
        }
    }

    // Встановлення нової випадкової точки призначення
    private void SetNewRandomDestination()
    {
        Vector3 newPosition = RandomNavMeshLocation();
        RaycastHit hit;

        // Перевірка, чи є перешкоди на шляху до нової точки, інакше встановлення нової точки
        if (Physics.Raycast(transform.position, newPosition - transform.position, out hit, Vector3.Distance(transform.position, newPosition)) &&
            hit.collider.CompareTag(wallTag))
        {
            Invoke("SetNewRandomDestination", Random.Range(1f, 3f));
        }
        else
        {
            randomDestination = newPosition;
            agent.SetDestination(randomDestination);
        }
    }

    // Рутина для зміни випадкової точки призначення кожні 2 секунди
    private IEnumerator RandomDestinationRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f); // Затримка
            SetNewRandomDestination(); // Встановлення нової випадкової точки призначення
        }
    }

    // Отримання випадкової точки на навмисному місці в межах walkRadius
    private Vector3 RandomNavMeshLocation()
    {
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);

        return hit.position;
    }

    // Обробка зіткнення
    private void OnCollisionStay2D(Collision2D other)
    {
        string entityTag = other.gameObject.tag;

        // Перевірка наявності кулдауну та часу з моменту останнього пошкодження
        if (!isCooldown && Time.time - lastDamageTime >= damageInterval)
        {
            Health health = other.gameObject.GetComponent<Health>();
            if (health != null)
            {
                // Зменшення здоров'я та запуск втечі
                health.Reduce((int)entityDamage, health.currentHealth);
                lastDamageTime = Time.time;

                isFleeing = true;

                StartCoroutine(FleeCooldown());

                // Встановлення нової випадкової точки та відміна слідування
                SetNewOppositePoint();
                isChasing = false;
                shouldFollowPlayer = false;
                agent.SetDestination(randomDestination);
            }
        }
    }

    // Рутина для втечі на певний час
    private IEnumerator FleeCooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(fleeCooldownDuration);
        isCooldown = false;
    }

    // Встановлення нової протилежної точки відносно гравця
    private void SetNewOppositePoint()
    {
        // Перевірка відстані до гравця
        if (Vector3.Distance(transform.position, player.position) <= chaseRadius)
        {
            // Визначення напрямку до гравця
            Vector3 directionToPlayer = transform.position - player.position;

            // Визначення протилежної точки
            Vector3 oppositePoint = transform.position + directionToPlayer.normalized * walkRadius;

            RaycastHit hit;

            // Перевірка, чи є перешкоди на шляху до протилежної точки, інакше встановлення нової точки
            if (Physics.Raycast(transform.position, oppositePoint - transform.position, out hit, walkRadius) &&
                hit.collider.CompareTag(wallTag))
            {
                Invoke("SetNewOppositePoint", Random.Range(1f, 3f));
            }
            else
            {
                randomDestination = oppositePoint;
            }
        }
        else
        {
            randomDestination = RandomNavMeshLocation();
        }
    }


}
