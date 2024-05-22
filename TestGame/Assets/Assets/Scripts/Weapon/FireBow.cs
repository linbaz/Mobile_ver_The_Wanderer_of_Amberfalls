using System.Collections;
using UnityEngine;

public class FireBow : Bow
{
    public float cooldownTime = 0.5f; // Время перезарядки между выстрелами
    private float currentCooldown = 0f; // Текущее время перезарядки
    private Camera globalMapCamera;

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

        // Создаем стрелу
        GameObject arrow = Instantiate(bullet, firePoint.position, Quaternion.identity);

        if (arrow != null)
        {
            // Устанавливаем направление стрелы
            arrow.GetComponent<ArrowBullet>().tw = this;
            arrow.GetComponent<ArrowBullet>().bulletSpeed = 5;
            arrow.GetComponent<ArrowBullet>().direction = directionToCursor.normalized;
            shootSound(); // Воспроизводим звук выстрела

            // Создаем область огня
            arrow.GetComponent<ArrowBullet>().OnHitTarget += CreateFireArea;
        }
    }

    private void CreateFireArea(Vector3 position)
    {
        // Создаем круглый коллайдер для области огня
        GameObject fireArea = new GameObject("FireArea");
        CircleCollider2D collider = fireArea.AddComponent<CircleCollider2D>();
        collider.radius = 3f; // Радиус области огня

        // Устанавливаем позицию области огня
        fireArea.transform.position = position;

        // Настраиваем коллайдер области огня
        collider.isTrigger = true;

        // Добавляем компонент обработки столкновений для нанесения урона
        fireArea.AddComponent<FireAreaCollider>();

        // Запускаем корутину для уничтожения области огня после определенного времени
        StartCoroutine(DestroyFireAreaAfterDelay(fireArea));
    }

    private IEnumerator DestroyFireAreaAfterDelay(GameObject fireArea)
    {
        // Ждем определенное время
        yield return new WaitForSeconds(5f);

        // Уничтожаем область огня
        Destroy(fireArea);
    }
}
