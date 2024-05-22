using UnityEngine;

public class FireAreaCollider : MonoBehaviour
{
    // ����, ��������� ������ � ������� ����
    public float damagePerSecond = 1f;

    // ��� ������������ � ������ �����������
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            return;

        if (other.gameObject.CompareTag("Arrow"))
            return;

        // ���� ����������� � ������
        if (other.CompareTag("Enemy"))
        {
            // ������� ���� ����� �� ������ ������� ���������� � ������� ����
            EntityStats enemy = other.GetComponent<EntityStats>();
            if (enemy != null)
            {
                enemy.GiveDamage(damagePerSecond * Time.deltaTime);
            }
        }
    }
}
