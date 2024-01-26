using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class RangedTest : MonoBehaviour
{
    [SerializeField] float chaseRadius = 10f; // ������ ������������� ������
    [SerializeField] float walkRadius = 5f;   // ������ ���������� ���������
    [SerializeField] float moveSpeed = 3f; // �������� ��������
    [SerializeField] string wallTag = "Walls"; // ��� ��������, ������� ��������� �������.

    private Transform player;                  // ������ �� ������
    private NavMeshAgent agent;                // ��������� NavMeshAgent
    private Vector3 randomDestination;         // ��������� ����� ����������
    private bool isShooting = false;            // ���� ��� ������������ ��������� �������������

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

        // ������������� �������� ��������
        agent.speed = moveSpeed;

        // ������� ������ �� ���� "Player"
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // ��������� ��������� ���������
        SetNewRandomDestination();
        StartCoroutine(RandomDestinationRoutine());
    }

    private void Update()
    {

        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // ���� ���������� �� ������ ������ ������� �������������, ���������� ������
            if (distanceToPlayer <= chaseRadius)
            {
                isShooting = true;
                SetNewRandomDestination();
            }
            else if (isShooting)
            {
                // ���� ������������� ���� ������������, �� ����� ����� �� �������, �������� � ��������� ���������
                isShooting = false;
                SetNewRandomDestination();
            }
            else if (!isShooting && Vector3.Distance(transform.position, randomDestination) < 0.3f)
            {
                // ���� �� ���������� ������ � �������� ��������� ����� ����������, ������������� �����
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
        // ����� ���������� ����� ����� ����������, ���������, ��� �� ����� �������.
        Vector3 newPosition = RandomNavMeshLocation();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, newPosition - transform.position, out hit, Vector3.Distance(transform.position, newPosition)) &&
            hit.collider.CompareTag(wallTag))
        {
            // ���� ���� �����, ���������� ����� ����� ��������� �����.
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
