using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    public EntityStats entityStats; // ������ �� ��������� EntityStats
    public Slider healthSlider; // ������ �� ��������� Slider

    void Update()
    {
        if (entityStats != null && healthSlider != null)
        {
            // ��������� �������� �������� � ������������ � ������� ���������
            healthSlider.value = entityStats.health;
        }
    }
}
