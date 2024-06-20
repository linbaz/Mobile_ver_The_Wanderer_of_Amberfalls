using UnityEngine;

public class Joystick : MonoBehaviour
{
    public Transform player;
    public float speed = 5.0f;
    public float deceleration = 10.0f; // Скорость замедления
    public Transform circle;
    public Transform outerCircle;
    public Transform hand;
    public Camera mainCamera;

    public float searchRadius = 15.0f; // Радиус поиска врагов
    public LayerMask enemyLayer; // Слой, на котором находятся враги

    private int joystickTouchId = -1;
    private Vector2 pointA;
    private Vector2 pointB;
    private Vector2 currentDirection = Vector2.zero;
    private Vector2 lastDirection = Vector2.zero;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began && touch.position.x < Screen.width / 2)
            {
                if (joystickTouchId == -1)
                {
                    joystickTouchId = touch.fingerId;
                    pointA = mainCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, mainCamera.nearClipPlane));
                    outerCircle.transform.position = pointA;
                    circle.transform.position = pointA;

                    circle.GetComponent<SpriteRenderer>().enabled = true;
                    outerCircle.GetComponent<SpriteRenderer>().enabled = true;
                }
            }

            if (touch.fingerId == joystickTouchId)
            {
                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    pointB = mainCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, mainCamera.nearClipPlane));
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    joystickTouchId = -1;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (joystickTouchId != -1)
        {
            Vector2 offset = pointB - (Vector2)outerCircle.transform.position;
            Vector2 direction = Vector2.ClampMagnitude(offset, 1.0f);
            moveCharacter(direction);

            circle.transform.position = new Vector2(outerCircle.transform.position.x + direction.x, outerCircle.transform.position.y + direction.y);

            lastDirection = direction; // Сохраняем последнее направление
        }
        else
        {
            circle.GetComponent<SpriteRenderer>().enabled = false;
            outerCircle.GetComponent<SpriteRenderer>().enabled = false;

            // Замедляем персонажа до полной остановки
            currentDirection = Vector2.Lerp(currentDirection, Vector2.zero, deceleration * Time.fixedDeltaTime);
            moveCharacter(currentDirection);

            // Обнуляем направление, если персонаж остановился
            if (currentDirection.magnitude < 0.01f)
            {
                currentDirection = Vector2.zero;
            }
        }
    }

    void moveCharacter(Vector2 direction)
    {
        player.Translate(direction * speed * Time.deltaTime);

        // Найти всех врагов в заданном радиусе
        Collider2D[] enemies = Physics2D.OverlapCircleAll(player.position, searchRadius, enemyLayer);

        // Найти ближайшего врага
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(player.position, enemy.transform.position);
            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = enemy.transform;
            }
        }

        // Если нашли ближайшего врага, направляем оружие на него
        if (closestEnemy != null)
        {
            Vector2 directionToEnemy = (closestEnemy.position - player.position).normalized;
            float angle = Mathf.Atan2(directionToEnemy.y, directionToEnemy.x) * Mathf.Rad2Deg;
            hand.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else
        {
            // Если врагов нет, направляем оружие в направлении джойстика
            if (direction != Vector2.zero)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                hand.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
    }

}
