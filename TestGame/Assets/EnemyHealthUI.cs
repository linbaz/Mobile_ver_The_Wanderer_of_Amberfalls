using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    public EntityStats entityStats; // —сылка на компонент EntityStats
    public Slider healthSlider; // —сылка на компонент Slider

    void Update()
    {
        if (entityStats != null && healthSlider != null)
        {
            // ќбновл€ем значение слайдера в соответствии с текущим здоровьем
            healthSlider.value = entityStats.health;
        }
    }
}
