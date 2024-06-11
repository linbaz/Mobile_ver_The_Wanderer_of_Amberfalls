using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FireBow : Bow
{
    public float cooldownTime = 0.5f; // ����� ����������� ����� ����������
    private float currentCooldown = 0f; // ������� ����� �����������

    public Button attackButton; // ������ �� ������ �� ������
    private bool isButtonPressed = false; // ���� ��� �������� ������� ������

    private void Start()
    {
        // ��������� ����������� ������� � ���������� ������
        attackButton.onClick.AddListener(OnAttackButtonPressed);
    }

    private void OnAttackButtonPressed()
    {
        isButtonPressed = true;
    }

    private void Update()
    {
        if (!inventory || !inventory.IsInventoryOpen())
        {
            if (!PauseMenu.GameIsPaused)
            {
                currentCooldown -= Time.deltaTime; // ��������� ����� �����������

                if (isButtonPressed && currentCooldown <= 0f)
                {
                    Shoot();
                    currentCooldown = cooldownTime; // ������������� ����� ����� �����������
                }
                else
                {
                    isButtonPressed = false;
                }
            }
        }
    }



    public override void Shoot()
    {
        // �������� ������� ������
        Vector3 playerPosition = transform.position;

        // �������� ������� firePoint (��� �������, ������������ ������� ����� ����������� ��������)
        Vector3 firePointPosition = firePoint.position;

        // ��������� ����������� �� ������ � firePoint
        Vector2 directionToFirePoint = (firePointPosition - playerPosition).normalized;

        // ������� ������
        GameObject arrow = Instantiate(bullet, playerPosition, Quaternion.identity);

        if (arrow != null)
        {
            // ������������� ����������� ������
            arrow.GetComponent<ArrowBullet>().tw = this;
            arrow.GetComponent<ArrowBullet>().bulletSpeed = 5;
            arrow.GetComponent<ArrowBullet>().direction = directionToFirePoint;
            shootSound(); // ������������� ���� ��������

            // ������� ������� ����
            arrow.GetComponent<ArrowBullet>().OnHitTarget += CreateFireArea;
        }
    }


    private void CreateFireArea(Vector3 position)
    {
        // ������� ������� ��������� ��� ������� ����
        GameObject fireArea = new GameObject("FireArea");
        CircleCollider2D collider = fireArea.AddComponent<CircleCollider2D>();
        collider.radius = 3f; // ������ ������� ����

        // ������������� ������� ������� ����
        fireArea.transform.position = position;

        // ����������� ��������� ������� ����
        collider.isTrigger = true;

        // ��������� ��������� ��������� ������������ ��� ��������� �����
        fireArea.AddComponent<FireAreaCollider>();

        // ��������� �������� ��� ����������� ������� ���� ����� ������������� �������
        StartCoroutine(DestroyFireAreaAfterDelay(fireArea));
    }

    private IEnumerator DestroyFireAreaAfterDelay(GameObject fireArea)
    {
        // ���� ������������ �����
        yield return new WaitForSeconds(5f);

        // ���������� ������� ����
        Destroy(fireArea);
    }
}
