using UnityEngine;

public class KeepZPosition : MonoBehaviour
{
    private float initialZPosition; // переменная для хранения начальной позиции по оси Z

    void Start()
    {
        // Сохраняем начальную позицию по оси Z при запуске сцены
        initialZPosition = transform.position.z;
    }

    void Update()
    {
        // Получаем текущую позицию объекта
        Vector3 currentPosition = transform.position;

        // Устанавливаем позицию по оси Z равной начальной позиции
        currentPosition.z = initialZPosition;

        // Устанавливаем новую позицию объекта
        transform.position = currentPosition;
    }
}
