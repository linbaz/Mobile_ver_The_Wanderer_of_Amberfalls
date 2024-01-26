using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats : MonoBehaviour
{

    public float health;
    public float maxHealth;


    public void GiveDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void GiveHealth(float addHealth)
    {
        health += addHealth;

        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

}