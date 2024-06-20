using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class MultiShotBow : Bow
{
    public int numberOfArrows = 3; // ���������� �����
    public float angleBetweenArrows = 10f; // ���� ����� ��������
    public float cooldownTime = 0.5f; // ����� ����������� ����� ����������

    private float currentCooldown = 0f; // ������� ����� �����������
    public Button attackButton; // ������ �� ������ �� ������
    private Coroutine attackCoroutine; // �������� ��� ��������� �����
    private bool isAttacking; // ���� ��� �������� ��������� �����

    private void Start()
    {
        isAttacking = false;

        // ��������� ����������� ������� ��� ������ �����
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
                // ��������� ����� �����������
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
                currentCooldown = cooldownTime; // ������������� ����� ����� �����������
            }
            yield return null; // ���� ���� ���� ����� ��������� ���������
        }
    }

    public override void Shoot()
    {
        // ���������� ������� ������ (����� �������� �� ������ ������, ���� ����� �� ������������)
        Vector3 playerPosition = transform.position;

        // ����������� �� ������ �� firePoint
        Vector2 directionToFirePoint = (firePoint.position - playerPosition).normalized;

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
            Vector2 spreadDirection = spreadRotation * directionToFirePoint;

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
