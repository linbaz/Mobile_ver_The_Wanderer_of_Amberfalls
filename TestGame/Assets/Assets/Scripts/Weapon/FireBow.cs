using System.Collections;
using UnityEngine;

public class FireBow : Bow
{
    public float cooldownTime = 0.5f; // ����� ����������� ����� ����������
    private float currentCooldown = 0f; // ������� ����� �����������
    private Camera globalMapCamera;

    private void Update()
    {
        if (!inventory || !inventory.IsInventoryOpen())
        {
            if (!PauseMenu.GameIsPaused)
            {
                currentCooldown -= Time.deltaTime; // ��������� ����� �����������

                if (Input.GetKey(KeyCode.Mouse0) && currentCooldown <= 0f)
                {
                    Shoot();
                    currentCooldown = cooldownTime; // ������������� ����� ����� �����������
                }
            }
        }  
    }

    public override void Shoot()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 directionToCursor = (mousePosition - firePoint.position).normalized;

        // ������� ������
        GameObject arrow = Instantiate(bullet, firePoint.position, Quaternion.identity);

        if (arrow != null)
        {
            // ������������� ����������� ������
            arrow.GetComponent<ArrowBullet>().tw = this;
            arrow.GetComponent<ArrowBullet>().bulletSpeed = 5;
            arrow.GetComponent<ArrowBullet>().direction = directionToCursor.normalized;
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
