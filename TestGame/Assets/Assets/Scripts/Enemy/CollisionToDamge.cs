using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollisionDamage : MonoBehaviour
{
    public float entityDamage;
    public float damageInterval = 2f; // Интервал между ударами
    private float lastDamageTime; // Время последнего удара
    private bool isCooldown = false;
    private float fleeCooldownDuration = 2f;

    private void OnCollisionStay2D(Collision2D other)
    {
        string entityTag = other.gameObject.tag;

        // Check if it's time to flee again
        if (!isCooldown && Time.time - lastDamageTime >= damageInterval)
        {
            Health health = other.gameObject.GetComponent<Health>();
            if (health != null)
            {
                health.Reduce((int)entityDamage, health.currentHealth);

                // Update the time of the last damage
                lastDamageTime = Time.time;
                StartCoroutine(FleeCooldown());
            }
        }
    }

    private IEnumerator FleeCooldown()
    {
        // Set the cooldown flag to true
        isCooldown = true;

        // Wait for the cooldown duration
        yield return new WaitForSeconds(fleeCooldownDuration);

        // Reset the cooldown flag
        isCooldown = false;
    }
}
