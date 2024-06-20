using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    public GameObject[] lootItems;
    public float health;



    public void GiveDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            DropLoot();
            Destroy(gameObject);
        }
    }

    private void DropLoot()
    {
        if (lootItems.Length > 0)
        {
            int randomItemIndex = Random.Range(0, lootItems.Length);

            GameObject spawnedLoot = Instantiate(lootItems[randomItemIndex], transform.position, Quaternion.identity);
        }
    }
}