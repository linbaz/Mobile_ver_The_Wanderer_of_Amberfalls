using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class UIInventoryItem : MonoBehaviour, IPointerClickHandler,
        IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler
    {
        [SerializeField]
        private Image itemImage; // Зображення предмету
        [SerializeField]
        private TMP_Text quantityTxt; // Текстове поле для відображення кількості предмету
        [SerializeField]
        private Image borderImage; // Зображення межі виділення предмету

        public event Action<UIInventoryItem> OnItemClicked, // Подія кліку на предмет
            OnItemDroppedOn, // Подія, коли предмет опущено на інший предмет
            OnItemBeginDrag, // Подія початку перетягування предмету
            OnItemEndDrag, // Подія завершення перетягування предмету
            OnRigthMouseBtnClick; // Подія правої кнопки миші на предмет

        private bool empty = true; // Прапорець порожнього слоту

        // Метод, який викликається при створенні об'єкта
        public void Awake()
        {
            ResetData(); // Скидання даних елемента
            Deselect(); // Зняття виділення
        }

        // Скидання даних елемента
        public void ResetData()
        {
            itemImage.gameObject.SetActive(false); // Вимкнення зображення предмету
            empty = true; // Встановлення прапорця порожнього слоту
        }

        // Зняття виділення
        public void Deselect()
        {
            borderImage.enabled = false; // Вимкнення зображення межі виділення
        }

        // Встановлення даних елемента
        public void SetData(Sprite sprite, int quantity)
        {
            itemImage.gameObject.SetActive(true); // Вмикання зображення предмету
            itemImage.sprite = sprite; // Встановлення зображення предмету
            quantityTxt.text = quantity + ""; // Встановлення кількості предмету
            empty = false; // Зняття прапорця порожнього слоту
        }

        // Виділення предмету
        public void Select()
        {
            borderImage.enabled = true; // Вмикання зображення межі виділення
        }

        // Обробник кліку на предмет
        public void OnPointerClick(PointerEventData pointerData)
        {
            if (pointerData.button == PointerEventData.InputButton.Left)
            {
                OnItemClicked?.Invoke(this);
                OnRigthMouseBtnClick?.Invoke(this); // Виклик події правої кнопки миші
            }
        }

        // Обробник початку перетягування предмету
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (empty)
                return;
            OnItemBeginDrag?.Invoke(this); // Виклик події початку перетягування предмету
        }

        // Обробник завершення перетягування предмету
        public void OnEndDrag(PointerEventData eventData)
        {
            OnItemEndDrag?.Invoke(this); // Виклик події завершення перетягування предмету
        }

        // Обробник опускання предмету на інший предмет
        public void OnDrop(PointerEventData eventData)
        {
            OnItemDroppedOn?.Invoke(this); // Виклик події опускання предмету на інший предмет
        }

        // Обробник перетягування предмету
        public void OnDrag(PointerEventData eventData)
        {

        }
    }
}
