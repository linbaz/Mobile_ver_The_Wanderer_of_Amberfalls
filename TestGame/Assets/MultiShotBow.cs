using UnityEngine;

public class MultiShotBow : Bow
{
    public int numberOfArrows = 3; // ���������� �����
    public float angleBetweenArrows = 10f; // ���� ����� ��������
    public float cooldownTime = 0.5f; // ����� ����������� ����� ����������
    private float currentCooldown = 0f; // ������� ����� �����������

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

        // ���� ����� ��������
        float angleBetweenArrowsRad = Mathf.Deg2Rad * angleBetweenArrows;
        float halfSpreadAngleRad = angleBetweenArrowsRad / 2f;

        // ��������� ��������� ���� ��� ������ ������
        float startAngle = -halfSpreadAngleRad * (numberOfArrows - 1);

        for (int i = 0; i < numberOfArrows; i++)
        {
            // ��������� ���� ��� ������� ������
            float currentAngle = startAngle + angleBetweenArrowsRad * i;

            // ������������ ����������� ������� �� ���� ����� ��������
            Quaternion spreadRotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * currentAngle);
            Vector2 spreadDirection = spreadRotation * directionToCursor;

            // ������� ������
            GameObject arrow = Instantiate(bullet, firePoint.position, spreadRotation);

            if (arrow != null)
            {
                arrow.GetComponent<ArrowBullet>().tw = this;
                arrow.GetComponent<ArrowBullet>().bulletSpeed = 5;
                arrow.GetComponent<ArrowBullet>().direction = spreadDirection.normalized;
                shootSound(); // ������������� ���� ��������
            }
        }
    }

}
