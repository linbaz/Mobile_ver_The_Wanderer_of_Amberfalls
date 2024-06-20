using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class MultiShotBow : Bow
{
    public int numberOfArrows = 3; // Количество стрел
    public float angleBetweenArrows = 10f; // Угол между стрелами
    public float cooldownTime = 0.5f; // Время перезарядки между выстрелами

    private float currentCooldown = 0f; // Текущее время перезарядки
    public Button attackButton; // Ссылка на кнопку на экране
    private Coroutine attackCoroutine; // Корутина для повторной атаки
    private bool isAttacking; // Флаг для проверки состояния атаки

    private void Start()
    {
        isAttacking = false;

        // Добавляем обработчики событий для кнопки атаки
        EventTrigger trigger = attackButton.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entryPointerDown = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerDown
        };
        entryPointerDown.callback.AddListener((data) => { OnAttackButtonDown(); });
        trigger.triggers.Add(entryPointerDown);

        EventTrigger.Entry entryPointerUp = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerUp
        };
        entryPointerUp.callback.AddListener((data) => { OnAttackButtonUp(); });
        trigger.triggers.Add(entryPointerUp);
    }

    private void Update()
    {
        if (!inventory || !inventory.IsInventoryOpen())
        {
            if (!PauseMenu.GameIsPaused)
            {
                // Уменьшаем время перезарядки
                if (currentCooldown > 0f)
                {
                    currentCooldown -= Time.deltaTime;
                }
            }
        }
    }

    private void OnAttackButtonDown()
    {
        if (attackCoroutine == null)
        {
            attackCoroutine = StartCoroutine(RepeatAttack());
        }
    }

    private void OnAttackButtonUp()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
            isAttacking = false;
        }
    }

    private IEnumerator RepeatAttack()
    {
        while (true)
        {
            if (currentCooldown <= 0f)
            {
                Shoot();
                currentCooldown = cooldownTime; // Устанавливаем новое время перезарядки
            }
            yield return null; // Ждем один кадр перед следующей проверкой
        }
    }

    public override void Shoot()
    {
        // Определяем позицию игрока (можно изменить на нужный объект, если игрок не используется)
        Vector3 playerPosition = transform.position;

        // Направление от игрока до firePoint
        Vector2 directionToFirePoint = (firePoint.position - playerPosition).normalized;

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
            Vector2 spreadDirection = spreadRotation * directionToFirePoint;

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

    public bool IsTouched(Touch touch)
    {
        RectTransform rectTransform = attackButton.GetComponent<RectTransform>();
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, touch.position, null, out localPoint);
        return rectTransform.rect.Contains(localPoint);
    }

    public void HandleTouch(Touch touch)
    {
        if (touch.phase == TouchPhase.Began)
        {
            OnAttackButtonDown();
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            OnAttackButtonUp();
        }
    }
}
