using Inventory.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Sword : MonoBehaviour
{
    public Animator animatorSword;
    public float delay = 0.5f; // Затримка між атаками
    public float damage = 5f;
    private bool isAttacking;
    public UIInventoryPage inventory;

    public Button attackButton; // Публічне поле для кнопки атаки

    private Coroutine attackCoroutine;

    private void Start()
    {
        isAttacking = false;

        // Додайте обробники подій для кнопки атаки
        attackButton.onClick.AddListener(StartAttack);

        // Використовуємо EventTrigger для обробки затискання і відпускання кнопки
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
            attackButton.onClick.AddListener(StartAttack);
        }
    }

    public void StartAttack()
    {
        if (!isAttacking)
        {
            Debug.Log("Атака");
            SwordAttack();
        }
    }

    private void SwordAttack()
    {
        animatorSword.SetTrigger("Attack");
        isAttacking = true;
        Collider2D swordCollider = GetComponent<Collider2D>();
        Bounds swordBounds = swordCollider.bounds;
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(swordBounds.center, swordBounds.size, 0f);
        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider != swordCollider && hitCollider.CompareTag("Enemy"))
            {
                EntityStats enemy = hitCollider.GetComponent<EntityStats>();

                if (enemy != null)
                {
                    enemy.GiveDamage(damage);
                }
            }
        }
    }

    private IEnumerator RepeatAttack()
    {
        while (isAttacking)
        {
            SwordAttack();
            yield return new WaitForSeconds(delay);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isAttacking)
        {
            if (!other.gameObject.CompareTag("Player"))
            {
                EntityStats enemy = other.GetComponent<EntityStats>();

                if (enemy != null && IsSwordCollidingWithEnemy(enemy))
                {
                    enemy.GiveDamage(damage);
                }
            }
        }
    }

    private bool IsSwordCollidingWithEnemy(EntityStats enemy)
    {
        Collider2D swordCollider = GetComponent<Collider2D>();

        return swordCollider.OverlapCollider(new ContactFilter2D(), new Collider2D[1]) > 0;
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

    public void CancelAttack()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
        isAttacking = false;
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
    }
}
