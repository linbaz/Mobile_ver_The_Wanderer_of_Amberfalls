using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArrowBullet : MonoBehaviour
{
    public float bulletSpeed;
    private Rigidbody2D rb;
    public ToWeapon tw;
    private int destroy = 3;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - transform.position).normalized;
        Vector2 directionOther = (direction * 1000).normalized;

        rb.velocity = directionOther * bulletSpeed;
        Debug.Log(mousePosition + " " + transform.position + " " + rb.velocity + " " + direction + " " + directionOther);

        // Вычисляем угол поворота пули
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);


        Invoke("DestroyTime", 4f);
    }

    void DestroyTime()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            EntityStats enemy = other.GetComponent<EntityStats>();
            if (enemy != null)
            {
                enemy.GiveDamage(tw.getDamage());
            }
            Destroy(gameObject);
        }       
    }
}
