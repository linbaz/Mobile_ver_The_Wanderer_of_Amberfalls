using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class UIInventoryDescription : MonoBehaviour
    {
        [SerializeField]
        private Image itemImage; // Зображення предмету
        [SerializeField]
        private TMP_Text title; // Текстове поле для відображення назви предмету
        [SerializeField]
        private TMP_Text description; // Текстове поле для відображення опису предмету

        // Метод, який викликається при створенні об'єкта
        public void Awake()
        {
            ResetDescription(); // Скидання опису
        }

        // Скидання опису
        public void ResetDescription()
        {
            itemImage.gameObject.SetActive(false); // Вимкнення зображення предмету
            title.text = ""; // Очищення назви предмету
            description.text = ""; // Очищення опису предмету
        }

        // Встановлення опису
        public void SetDescription(Sprite sprite, string itemName,
            string itemDescription)
        {
            itemImage.gameObject.SetActive(true); // Вмикання зображення предмету
            itemImage.sprite = sprite; // Встановлення зображення предмету
            title.text = itemName; // Встановлення назви предмету
            description.text = itemDescription; // Встановлення опису предмету
        }
    }
}
