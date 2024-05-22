using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.UI
{
    public class UIInventoryPage : MonoBehaviour
    {
        [SerializeField]
        private UIInventoryItem inventoryItemUIPrefab; // Префаб UI елемента інвентаря

        [SerializeField]
        private RectTransform contentPanel; // Панель контенту інвентаря

        [SerializeField]
        private UIInventoryDescription itemDescription; // Опис предмету

        [SerializeField]
        private MouseFollower mouseFollower; // Слідуючий за мишею об'єкт

        List<UIInventoryItem> listOfUIItems = new List<UIInventoryItem>(); // Список UI елементів інвентаря

        private bool isInventoryOpen = false; // Прапорець відкриття інвентаря

        public event Action<int> OnDescriptionRequested, // Подія запиту опису предмету
            OnItemActionRequested, // Подія запиту виконання дії над предметом
            OnStartDragging; // Подія початку перетягування предмету

        public event Action<int, int> OnSwapItems; // Подія обміну предметами

        private int currentlyDraggedItemIndex = -1; // Індекс перетягуваного предмету

        [SerializeField]
        private ItemActionPanel actionPanel; // Панель дій над предметом

        // Метод, який викликається при створенні об'єкта
        private void Awake()
        {
            Hide(); // Приховання інвентаря при запуску
            mouseFollower.Toggle(false); // Вимкнення слідування за мишею
            itemDescription.ResetDescription(); // Скидання опису предмету
        }

        // Ініціалізація UI інвентаря
        public void InitializeInventoryUI(int inventorysize)
        {
            for (int i = 0; i < inventorysize; i++)
            {
                UIInventoryItem uiItem = Instantiate(inventoryItemUIPrefab, Vector3.zero, Quaternion.identity, contentPanel);
                uiItem.transform.SetParent(contentPanel);
                listOfUIItems.Add(uiItem);
                uiItem.OnItemClicked += HandleItemSelection;
                uiItem.OnItemBeginDrag += HandleBeginDrag;
                uiItem.OnItemDroppedOn += HandleSwap;
                uiItem.OnItemEndDrag += HandleEndDrag;
                uiItem.OnRigthMouseBtnClick += HandleShowItemActions;
            }
        }

        // Оновлення даних елемента UI інвентаря
        public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
        {
            if (listOfUIItems.Count > itemIndex)
            {
                listOfUIItems[itemIndex].SetData(itemImage, itemQuantity);
            }
        }

        // Обробник відображення можливих дій над предметом
        private void HandleShowItemActions(UIInventoryItem inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1)
            {
                return;
            }
            OnItemActionRequested?.Invoke(index);
        }

        // Обробник завершення перетягування предмету
        private void HandleEndDrag(UIInventoryItem inventoryItemUI)
        {
            ResetDraggedItem();
        }

        // Обробник обміну предметами
        private void HandleSwap(UIInventoryItem inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1)
            {
                return;
            }
            OnSwapItems?.Invoke(currentlyDraggedItemIndex, index);
            HandleItemSelection(inventoryItemUI);
        }

        // Скидання перетягуваного предмету
        private void ResetDraggedItem()
        {
            mouseFollower.Toggle(false);
            currentlyDraggedItemIndex = -1;
        }

        // Обробник початку перетягування предмету
        private void HandleBeginDrag(UIInventoryItem inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1)
                return;
            currentlyDraggedItemIndex = index;
            HandleItemSelection(inventoryItemUI);
            OnStartDragging?.Invoke(index);
        }

        // Створення елемента, який перетягується
        public void CreateDraggedItem(Sprite sprite, int quantity)
        {
            mouseFollower.Toggle(true);
            mouseFollower.SetData(sprite, quantity);
        }

        // Обробник вибору предмету
        private void HandleItemSelection(UIInventoryItem inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1)
                return;
            OnDescriptionRequested?.Invoke(index);
        }

        // Відображення інвентаря
        public void Show()
        {
            gameObject.SetActive(true);
            itemDescription.ResetDescription();
            isInventoryOpen = true;
            ResetSelection();
        }

        // Скидання вибору
        public void ResetSelection()
        {
            itemDescription.ResetDescription();
            DeselectAllItems();
        }

        // Додавання дії над предметом
        public void AddAction(string actionName, Action performAction)
        {
            actionPanel.AddButon(actionName, performAction);
        }

        // Показ можливих дій над предметом
        public void ShowItemAction(int itemIndex)
        {
            actionPanel.Toggle(true);
            actionPanel.transform.position = listOfUIItems[itemIndex].transform.position;
        }

        // Зняття вибору з усіх предметів
        private void DeselectAllItems()
        {
            foreach (UIInventoryItem item in listOfUIItems)
            {
                item.Deselect();
            }
            actionPanel.Toggle(false);
        }

        // Приховання інвентаря
        public void Hide()
        {
            actionPanel.Toggle(false);
            gameObject.SetActive(false);
            isInventoryOpen = false;
            ResetDraggedItem();
        }

        // Перевірка, чи відкритий інвентар
        public bool IsInventoryOpen()
        {
            return isInventoryOpen;
        }

        // Оновлення опису предмету та вибору в інвентарі
        internal void UpdateDescription(int itemIndex, Sprite itemImage, string name, string description)
        {
            itemDescription.SetDescription(itemImage, name, description);
            DeselectAllItems();
            listOfUIItems[itemIndex].Select();
        }

        // Скидання даних всіх елементів інвентаря
        internal void ReselAllItems()
        {
            foreach (var item in listOfUIItems)
            {
                item.ResetData();
                item.Deselect();
            }
        }
    }
}
