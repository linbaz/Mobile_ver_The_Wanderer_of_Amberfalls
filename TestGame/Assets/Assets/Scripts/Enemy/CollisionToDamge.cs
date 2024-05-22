using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// ����, �� ������� �� �������� ����� ��'����� �� ��� ��������
public class CollisionDamage : MonoBehaviour
{
    // �����������, ��� ����� ��'���
    public float entityDamage;

    // �������� �� ��������� �����������
    public float damageInterval = 2f;

    // ��� ���������� �������� �����������
    private float lastDamageTime;

    // ��������� ��� ���������� ���� �����������
    private bool isCooldown = false;

    // ��������� ����������� ���� �������� �����������
    private float fleeCooldownDuration = 2f;

    // �������� ��������
    private void OnCollisionStay2D(Collision2D other)
    {
        // ��������� ���� ������ ��'����, ���� ��������
        string entityTag = other.gameObject.tag;

        // ��������, �� ����� ������� ����������� � ����� ������ �� �� ����� ��������
        if (!isCooldown && Time.time - lastDamageTime >= damageInterval)
        {
            // ��������� ���������� Health ��'����
            Health health = other.gameObject.GetComponent<Health>();

            // ��������, �� ���� ��������� Health �� ��������� ������'�
            if (health != null)
            {
                health.Reduce((int)entityDamage, health.currentHealth);

                // ��������� ���� ���������� �������� ����������� �� ������� �����������
                lastDamageTime = Time.time;
                StartCoroutine(FleeCooldown());
            }
        }
    }

    // ������� ��� ����������� ���� �������� �����������
    private IEnumerator FleeCooldown()
    {
        isCooldown = true;

        yield return new WaitForSeconds(fleeCooldownDuration);

        isCooldown = false;
    }
}
