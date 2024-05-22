using Inventory.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas; // Канва для відслідковування позиції миші

    [SerializeField]
    private UIInventoryItem item; // Об'єкт предмету, який слідкує за мишою

    // Метод, який викликається при створенні об'єкта
    public void Awake()
    {
        canvas = transform.root.GetComponent<Canvas>(); // Отримання компонента канви
        item = GetComponentInChildren<UIInventoryItem>(); // Отримання об'єкта предмету у дочірніх елементах
    }

    // Встановлення даних предмету (зображення та кількість)
    public void SetData(Sprite sprite, int quantity)
    {
        item.SetData(sprite, quantity);
    }

    // Оновлення позиції об'єкта відносно позиції миші
    private void Update()
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            Input.mousePosition,
            canvas.worldCamera,
            out position);
        transform.position = canvas.transform.TransformPoint(position);
    }

    // Зміна видимості об'єкта
    public void Toggle(bool val)
    {
        Debug.Log($"Item toggled {val}");
        gameObject.SetActive(val);
    }
}
