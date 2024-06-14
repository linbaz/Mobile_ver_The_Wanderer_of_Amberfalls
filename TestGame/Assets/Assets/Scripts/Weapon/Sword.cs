using Inventory.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Sword : MonoBehaviour
{
    public Animator animatorSword;
    public float delay = 0f;
    public float damage = 5f;
    private bool attackBlocked;
    public UIInventoryPage inventory;

    public Button attackButton; // Публічне поле для кнопки атаки

    private void Start()
    {
        attackBlocked = false;

        // Додайте обробник події для кнопки атаки
        attackButton.onClick.AddListener(StartAttack);
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
        if (!attackBlocked)
        {
            Debug.Log("Атака");
            SwordAttack();
        }
    }

    private void SwordAttack()
    {
        animatorSword.SetTrigger("Attack");
        attackBlocked = true;
        StartCoroutine(DelayAttack());
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

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delay);
        attackBlocked = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (attackBlocked)
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

    public void CancelAttack()
    {
        StopAllCoroutines();
        attackBlocked = false;
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
    }
}