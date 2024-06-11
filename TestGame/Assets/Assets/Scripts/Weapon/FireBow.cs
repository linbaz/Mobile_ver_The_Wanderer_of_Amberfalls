using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FireBow : Bow
{
    public float cooldownTime = 0.5f; // Время перезарядки между выстрелами
    private float currentCooldown = 0f; // Текущее время перезарядки

    public Button attackButton; // Ссылка на кнопку на экране
    private bool isButtonPressed = false; // Флаг для проверки нажатия кнопки

    private void Start()
    {
        // Назначаем обработчики нажатия и отпускания кнопки
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
                currentCooldown -= Time.deltaTime; // Уменьшаем время перезарядки

                if (isButtonPressed && currentCooldown <= 0f)
                {
                    Shoot();
                    currentCooldown = cooldownTime; // Устанавливаем новое время перезарядки
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
        // Получаем позицию игрока
        Vector3 playerPosition = transform.position;

        // Получаем позицию firePoint (это позиция, относительно которой будет происходить стрельба)
        Vector3 firePointPosition = firePoint.position;

        // Вычисляем направление от игрока к firePoint
        Vector2 directionToFirePoint = (firePointPosition - playerPosition).normalized;

        // Создаем стрелу
        GameObject arrow = Instantiate(bullet, playerPosition, Quaternion.identity);

        if (arrow != null)
        {
            // Устанавливаем направление стрелы
            arrow.GetComponent<ArrowBullet>().tw = this;
            arrow.GetComponent<ArrowBullet>().bulletSpeed = 5;
            arrow.GetComponent<ArrowBullet>().direction = directionToFirePoint;
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
