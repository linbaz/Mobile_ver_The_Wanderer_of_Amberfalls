using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArrowBullet : MonoBehaviour
{
    public event System.Action<Vector3> OnHitTarget; // Событие для обработки попадания стрелы в цель

    public float bulletSpeed;
    public Vector2 direction;
    private Rigidbody2D rb;
    public ToWeapon tw;
    private int destroy = 3;
    private Vector3 previose_position;

    private void Start()
    {
        previose_position = transform.position;
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction.normalized * bulletSpeed; // Используем переданное направление
        Invoke("DestroyTime", 4f);

        // Вычисляем угол поворота
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }


    private void Update()
    {
        float betweenDistance = Vector3.Distance(transform.position, previose_position);
        Debug.DrawLine(previose_position, transform.position, Color.red, 0.5f);
        RaycastHit hit;

        if (Physics.Raycast(previose_position, (Vector3) transform.position - previose_position, out hit))
        {
            if (hit.collider.gameObject.CompareTag("Walls"))
            {
                Debug.Log("Ray with: " + hit.collider.gameObject.name);
                Destroy(gameObject);
            }
        }

        previose_position = transform.position; 

    }

    void DestroyTime()
    {
        Destroy(gameObject);
    }

     
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            return;

        if (other.gameObject.CompareTag("Arrow"))
            return;

        if (other.GetComponent<ArrowBullet>() != null)
            return;

        if (other.GetComponent<FireAreaCollider>() != null)
            return;


        EntityStats enemy = other.GetComponent<EntityStats>();
        if (enemy != null)
        {
            enemy.GiveDamage(tw.getDamage());
            OnHitTarget?.Invoke(transform.position);
        }
        Debug.Log("HUI: " + gameObject.name);
        Destroy(gameObject);
    }
}
