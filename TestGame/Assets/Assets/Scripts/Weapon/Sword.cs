using Inventory.UI;
using System.Collections;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public Animator animatorSword;
    public float delay = 0.3f;
    public float damage = 5f;
    private bool attackBlocked;
    public UIInventoryPage inventory;


    private void Start()
    {
        attackBlocked = false;
    }

    private void Update()
    {
        if (!inventory || !inventory.IsInventoryOpen())
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                SwordAttack();
            }
        }
    }

    private void SwordAttack()
    {
        if (attackBlocked)
            return;

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
        if (Input.GetKey(KeyCode.Mouse0))
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