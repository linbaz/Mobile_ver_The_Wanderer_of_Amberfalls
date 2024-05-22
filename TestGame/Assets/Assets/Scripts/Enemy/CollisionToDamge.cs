using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Клас, що відповідає за завдання шкоди об'єктам під час зіткнення
public class CollisionDamage : MonoBehaviour
{
    // Пошкодження, яке завдає об'єкт
    public float entityDamage;

    // Інтервал між завданням пошкодження
    public float damageInterval = 2f;

    // Час останнього завдання пошкодження
    private float lastDamageTime;

    // Прапорець для визначення часу охолодження
    private bool isCooldown = false;

    // Тривалість охолодження після завдання пошкодження
    private float fleeCooldownDuration = 2f;

    // Обробник зіткнення
    private void OnCollisionStay2D(Collision2D other)
    {
        // Отримання тегу іншого об'єкта, який зіткнувся
        string entityTag = other.gameObject.tag;

        // Перевірка, чи можна завдати пошкодження в даний момент та чи минув інтервал
        if (!isCooldown && Time.time - lastDamageTime >= damageInterval)
        {
            // Отримання компонента Health об'єкта
            Health health = other.gameObject.GetComponent<Health>();

            // Перевірка, чи існує компонент Health та зменшення здоров'я
            if (health != null)
            {
                health.Reduce((int)entityDamage, health.currentHealth);

                // Оновлення часу останнього завдання пошкодження та початок охолодження
                lastDamageTime = Time.time;
                StartCoroutine(FleeCooldown());
            }
        }
    }

    // Корутин для охолодження після завдання пошкодження
    private IEnumerator FleeCooldown()
    {
        isCooldown = true;

        yield return new WaitForSeconds(fleeCooldownDuration);

        isCooldown = false;
    }
}
