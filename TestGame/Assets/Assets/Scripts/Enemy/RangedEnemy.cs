using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : MonoBehaviour
{
    public Transform player;
    public GameObject bullet;

    private float shootCooldown;
    public float startShootCooldown;

    void Start()
    {
        shootCooldown = startShootCooldown;

    }

    void Update()
    {
        Vector2 direction = new Vector2(player.position.x - transform.position.x, player.position.y - transform.position.y);

        transform.up = direction;

        if (shootCooldown <= 0) 
        { 
            Instantiate(bullet, transform.position, transform.rotation);
            shootCooldown = startShootCooldown;
        }
        else
        {
            shootCooldown -= Time.deltaTime;
        }
    }
}
