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

    public void SwordAttack()
    {
        if (attackBlocked)
            return;

        animatorSword.SetTrigger("Attack");
        attackBlocked = true;

        // Запускаем задержку атаки
        StartCoroutine(DelayAttack());
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

                if (enemy != null)
                {
                   enemy.GiveDamage(damage);
                }
            }
        }
            
    }

    public void CancelAttack()
    {
        StopAllCoroutines();
        attackBlocked = false;
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
    }
}
