using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Клас, який представляє предмет в данжі
public class ItemInDungeon : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private BoxCollider2D itemCollider;

    [SerializeField]
    int health = 3;
    [SerializeField]
    bool nonDestructible;

    [SerializeField]
    private GameObject hitFeedback, destoyFeedback;

    // Подія, яка спрацьовує при отриманні удару
    public UnityEvent OnGetHit { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    // Ініціалізація предмета за даними з ItemData
    public void Initialize(ItemData itemData)
    {
        // Встановлення спрайту
        spriteRenderer.sprite = itemData.sprite;
        // Встановлення зміщення спрайту
        spriteRenderer.transform.localPosition = new Vector2(0.5f * itemData.size.x, 0.5f * itemData.size.y);
        itemCollider.size = itemData.size;
        itemCollider.offset = spriteRenderer.transform.localPosition;

        if (itemData.nonDestructible)
            nonDestructible = true;

        this.health = itemData.health;
    }

    // Метод для отримання удару та обробки відповідно до поточного стану предмета
    public void GetHit(int damage, GameObject damageDealer)
    {
        if (nonDestructible)
            return;

        // Якщо здоров'я більше 1, відображення ефекту удару, інакше - ефект знищення
        if (health > 1)
            Instantiate(hitFeedback, spriteRenderer.transform.position, Quaternion.identity);
        else
            Instantiate(destoyFeedback, spriteRenderer.transform.position, Quaternion.identity);

        ReduceHealth();
    }

    // Метод для зменшення здоров'я предмета
    private void ReduceHealth()
    {
        health--;
        // Якщо здоров'я стало менше або дорівнює 0, знищення предмета
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
