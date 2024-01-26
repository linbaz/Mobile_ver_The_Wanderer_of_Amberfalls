using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private bool damageGiven = false;
    public float health;
    public float maxHealth;

    public HealthBar healthBar;

    public void Start()
    {
        health = maxHealth;
        healthBar = GetComponent<HealthBar>();
        healthBar.SetMaxHealth((int)maxHealth);
    }

    public void GiveDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        damageGiven = true;
        healthBar.SetHealth((int)health);
    }

    public void GiveHealth(float addHealth)
    {
        health += addHealth;

        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public bool IsDamageGiven()
    {
        return damageGiven;
    }

}