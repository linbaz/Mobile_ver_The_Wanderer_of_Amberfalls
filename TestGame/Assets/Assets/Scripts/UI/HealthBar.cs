using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private FloatValueSO floatValue;
    public Slider slider;

    public void SetMaxHealth(int floatValue)
    {
        slider.maxValue = floatValue;
        slider.value = floatValue;
    }

    public void SetHealth(float floatValue)
    {
        slider.value = floatValue;
    }
}
