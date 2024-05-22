using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// ����, ���� ����������� ������� � ����
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

    // ����, ��� ��������� ��� �������� �����
    public UnityEvent OnGetHit { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    // ����������� �������� �� ������ � ItemData
    public void Initialize(ItemData itemData)
    {
        // ������������ �������
        spriteRenderer.sprite = itemData.sprite;
        // ������������ ������� �������
        spriteRenderer.transform.localPosition = new Vector2(0.5f * itemData.size.x, 0.5f * itemData.size.y);
        itemCollider.size = itemData.size;
        itemCollider.offset = spriteRenderer.transform.localPosition;

        if (itemData.nonDestructible)
            nonDestructible = true;

        this.health = itemData.health;
    }

    // ����� ��� ��������� ����� �� ������� �������� �� ��������� ����� ��������
    public void GetHit(int damage, GameObject damageDealer)
    {
        if (nonDestructible)
            return;

        // ���� ������'� ����� 1, ����������� ������ �����, ������ - ����� ��������
        if (health > 1)
            Instantiate(hitFeedback, spriteRenderer.transform.position, Quaternion.identity);
        else
            Instantiate(destoyFeedback, spriteRenderer.transform.position, Quaternion.identity);

        ReduceHealth();
    }

    // ����� ��� ��������� ������'� ��������
    private void ReduceHealth()
    {
        health--;
        // ���� ������'� ����� ����� ��� ������� 0, �������� ��������
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
