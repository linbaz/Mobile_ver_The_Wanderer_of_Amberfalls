using UnityEngine;

public class Joystick : MonoBehaviour
{
    public Transform player;
    public float speed = 5.0f;
    public float deceleration = 10.0f; // скорость замедления
    private bool touchStart = false;
    private Vector2 pointA;
    private Vector2 pointB;

    public Transform circle;
    public Transform outerCircle;
    public Transform hand;

    public Camera mainCamera;

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
        if (Input.GetMouseButtonDown(0))
        {
            pointA = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.nearClipPlane));
            if (Input.mousePosition.x < Screen.width / 2) // проверяем, что клик был на левой части экрана
            {
                outerCircle.transform.position = pointA;
                circle.transform.position = pointA;

                circle.GetComponent<SpriteRenderer>().enabled = true;
                outerCircle.GetComponent<SpriteRenderer>().enabled = true;

                touchStart = true;
            }
        }
        if (Input.GetMouseButton(0))
        {
            if (touchStart)
            {
                pointB = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.nearClipPlane));
            }
        }
        else
        {
            touchStart = false;
        }
    }

    private void FixedUpdate()
    {
        if (touchStart)
        {
            Vector2 offset = pointB - (Vector2)outerCircle.transform.position;
            Vector2 direction = Vector2.ClampMagnitude(offset, 1.0f);
            moveCharacter(direction);

            circle.transform.position = new Vector2(outerCircle.transform.position.x + direction.x, outerCircle.transform.position.y + direction.y);

            lastDirection = direction; // сохраняем последнее направление
        }
        else
        {
            circle.GetComponent<SpriteRenderer>().enabled = false;
            outerCircle.GetComponent<SpriteRenderer>().enabled = false;

            // замедляем персонажа до полной остановки
            currentDirection = Vector2.Lerp(currentDirection, Vector2.zero, deceleration * Time.fixedDeltaTime);
            moveCharacter(currentDirection);

            // обнуляем направление, если персонаж остановился
            if (currentDirection.magnitude < 0.01f)
            {
                currentDirection = Vector2.zero;
            }
        }
    }

    void moveCharacter(Vector2 direction)
    {
        player.Translate(direction * speed * Time.deltaTime);

        if (direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            hand.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    public bool IsTouched(Touch touch)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, touch.position, null, out localPoint);
        return rectTransform.rect.Contains(localPoint);
    }

    public void HandleTouch(Touch touch)
    {
        if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
        {
            pointB = mainCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, mainCamera.nearClipPlane));
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            touchStart = false;
        }
    }
}
