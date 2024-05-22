using UnityEngine;

public class MultiShotBow : Bow
{
    public int numberOfArrows = 3; // Количество стрел
    public float angleBetweenArrows = 10f; // Угол между стрелами
    public float cooldownTime = 0.5f; // Время перезарядки между выстрелами
    private float currentCooldown = 0f; // Текущее время перезарядки

    private void Update()
    {
        if (!inventory || !inventory.IsInventoryOpen())
        {
            if (!PauseMenu.GameIsPaused)
            {
                currentCooldown -= Time.deltaTime; // Уменьшаем время перезарядки

                if (Input.GetKey(KeyCode.Mouse0) && currentCooldown <= 0f)
                {
                    Shoot();
                    currentCooldown = cooldownTime; // Устанавливаем новое время перезарядки
                }
            }
        }
    }

    public override void Shoot()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 directionToCursor = (mousePosition - firePoint.position).normalized;

        // Угол между стрелами
        float angleBetweenArrowsRad = Mathf.Deg2Rad * angleBetweenArrows;
        float halfSpreadAngleRad = angleBetweenArrowsRad / 2f;

        // Вычисляем начальный угол для первой стрелы
        float startAngle = -halfSpreadAngleRad * (numberOfArrows - 1);

        for (int i = 0; i < numberOfArrows; i++)
        {
            // Вычисляем угол для текущей стрелы
            float currentAngle = startAngle + angleBetweenArrowsRad * i;

            // Поворачиваем направление курсора на угол между стрелами
            Quaternion spreadRotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * currentAngle);
            Vector2 spreadDirection = spreadRotation * directionToCursor;

            // Создаем стрелу
            GameObject arrow = Instantiate(bullet, firePoint.position, spreadRotation);

            if (arrow != null)
            {
                arrow.GetComponent<ArrowBullet>().tw = this;
                arrow.GetComponent<ArrowBullet>().bulletSpeed = 5;
                arrow.GetComponent<ArrowBullet>().direction = spreadDirection.normalized;
                shootSound(); // Воспроизводим звук выстрела
            }
        }
    }

}
