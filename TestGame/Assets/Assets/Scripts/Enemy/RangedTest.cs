using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// Клас для реалізації руху та стрільби ворожого об'єкта
public class RangedTest : MonoBehaviour
{
    [SerializeField] float chaseRadius = 10f;   // Радіус преслідування гравця
    [SerializeField] float walkRadius = 5f;     // Радіус блукання
    [SerializeField] float moveSpeed = 3f;      // Швидкість руху
    [SerializeField] string wallTag = "Walls";  // Тег для стін

    private Transform player;                   // Посилання на об'єкт гравця
    private NavMeshAgent agent;                 // Компонент NavMeshAgent для руху по мережі навігації
    private Vector3 randomDestination;          // Випадкова точка призначення
    private bool isShooting = false;            // Прапорець для визначення, чи ворог стріляє

    public GameObject bullet;                   // Префаб кулі
    private float shootCooldown;                // Затримка між пострілами
    public float startShootCooldown;             // Початкова затримка між пострілами

    // Ініціалізація
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Знаходження гравця за тегом
        shootCooldown = startShootCooldown; // Ініціалізація затримки між пострілами
        agent = GetComponent<NavMeshAgent>(); // Отримання компонента NavMeshAgent
        agent.updateRotation = false; // Вимкнення автоматичного обертання
        agent.updateUpAxis = false;
        agent.transform.rotation = Quaternion.Euler(0, 0, 0); // Обнулення обертання

        agent.speed = moveSpeed; // Встановлення швидкості руху

        player = GameObject.FindGameObjectWithTag("Player").transform; // Знову знаходження гравця за тегом

        SetNewRandomDestination(); // Встановлення випадкової точки призначення
        StartCoroutine(RandomDestinationRoutine()); // Запуск корутини блукання
    }

    // Оновлення
    private void Update()
    {
        // Перевірка наявності гравця
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position); // Визначення відстані до гравця

            // Якщо гравець знаходиться у радіусі преслідування
            if (distanceToPlayer <= chaseRadius)
            {
                isShooting = true; // Ворог готується до стрільби
                SetNewRandomDestination(); // Встановлення нової випадкової точки призначення
            }
            // Якщо ворог вже стріляє і гравець виходить за межі радіусу преслідування
            else if (isShooting)
            {
                isShooting = false; // Ворог перестає стріляти
                SetNewRandomDestination(); // Встановлення нової випадкової точки призначення
            }
            // Якщо ворог не стріляє і вже знаходиться призначенні
            else if (!isShooting && Vector3.Distance(transform.position, randomDestination) < 0.3f)
            {
                SetNewRandomDestination(); // Встановлення нової випадкової точки призначення
            }

            // Якщо ворог стріляє
            if (isShooting)
            {
                              

                // Якщо затримка між пострілами завершилась
                if (shootCooldown <= 0)
                {
                    Instantiate(bullet, transform.position, transform.rotation); // Створення кулі
                    shootCooldown = startShootCooldown; // Встановлення нової затримки між пострілами
                }
                else
                {
                    shootCooldown -= Time.deltaTime; // Зменшення залишкової затримки
                }
            }
        }
        else
        {
            isShooting = false; // Якщо гравець не знайдений, ворог не стріляє
        }
    }

    // Встановлення нової випадкової точки призначення
    private void SetNewRandomDestination()
    {
        // Генерація нової випадкової точки
        Vector3 newPosition = RandomNavMeshLocation();
        RaycastHit hit;

        // Перевірка наявності стін на шляху до нової точки
        if (Physics.Raycast(transform.position, newPosition - transform.position, out hit, Vector3.Distance(transform.position, newPosition)) &&
            hit.collider.CompareTag(wallTag))
        {
            Invoke("SetNewRandomDestination", Random.Range(1f, 3f)); // Якщо є, спробувати ще раз
        }
        else
        {
            randomDestination = newPosition; // Встановлення нової точки призначення
            agent.SetDestination(randomDestination); // Встановлення нової точки для NavMeshAgent
        }
    }

    // Випадкова точка в межах walkRadius
    private Vector3 RandomNavMeshLocation()
    {
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);

        return hit.position;
    }

    // Корутина для встановлення нової випадкової точки призначення
    private IEnumerator RandomDestinationRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f); // Затримка
            SetNewRandomDestination(); // Встановлення нової випадкової точки призначення
        }
    }
}
