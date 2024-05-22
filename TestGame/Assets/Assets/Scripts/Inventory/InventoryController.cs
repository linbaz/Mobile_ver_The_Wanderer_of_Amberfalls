using Inventory.Model;
using Inventory.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        public UIInventoryPage inventoryUI; // Посилання на UI інвентар

        public InventorySO inventoryData; // Дані інвентаря (ScriptableObject)

        public List<InventoryItem> initialItems = new List<InventoryItem>(); // Початкові предмети

        // Метод, який викликається при старті
        public void Start()
        {
            PrepareUI(); // Підготовка UI
            PrepareInventoryData(); // Підготовка даних інвентаря
        }

        // Підготовка даних інвентаря
        private void PrepareInventoryData()
        {
            inventoryData.Initialize(); // Ініціалізація даних інвентаря
            inventoryData.OnInventoryUpdated += UpdateInventoryUI; // Підписка на подію оновлення інвентаря
            foreach (InventoryItem item in initialItems)
            {
                if (item.IsEmpty)
                    continue;
                inventoryData.AddItem(item); // Додавання початкових предметів до інвентаря
            }
        }

        // Оновлення інтерфейсу інвентаря
        public void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            inventoryUI.ReselAllItems(); // Скидання всіх елементів UI
            foreach (var item in inventoryState)
            {
                inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity); // Оновлення даних елементів UI
            }
        }

        // Підготовка UI інвентаря
        private void PrepareUI()
        {
            inventoryUI.InitializeInventoryUI(inventoryData.Size); // Ініціалізація UI інвентаря
            inventoryUI.OnDescriptionRequested += HandleDescriptionRequest; // Підписка на подію запиту опису предмету
            inventoryUI.OnSwapItems += HandleSwapItems; // Підписка на подію обміну предметами
            inventoryUI.OnStartDragging += HandleDragging; // Підписка на подію початку перетягування предмету
            inventoryUI.OnItemActionRequested += HandleItemActionRequest; // Підписка на подію запиту виконання дії над предметом
        }

        // Обробник події запиту виконання дії над предметом
        private void HandleItemActionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;

            IItemAction itemAction = inventoryItem.item as IItemAction;
            if (itemAction != null)
            {
                inventoryUI.ShowItemAction(itemIndex);
                inventoryUI.AddAction(itemAction.ActionName, () => PerformAction(itemIndex));
            }

            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
            if (destroyableItem != null)
            {
                inventoryUI.AddAction("Drop", () => DropItem(itemIndex, inventoryItem.quantity));
            }
        }

        // Викидання предмета
        public void DropItem(int itemIndex, int quantity)
        {
            inventoryData.RemoveItem(itemIndex, quantity);
            inventoryUI.ResetSelection();
        }

        // Виконання дії над предметом
        public void PerformAction(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;

            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
            if (destroyableItem != null)
            {
                inventoryData.RemoveItem(itemIndex, 1);
            }

            IItemAction itemAction = inventoryItem.item as IItemAction;
            if (itemAction != null)
            {
                itemAction.PerformAction(gameObject);
                if (inventoryData.GetItemAt(itemIndex).IsEmpty)
                    inventoryUI.ResetSelection();
            }
        }

        // Обробник події початку перетягування предмету
        private void HandleDragging(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;
            inventoryUI.CreateDraggedItem(inventoryItem.item.ItemImage, inventoryItem.quantity);
        }

        // Обробник події обміну предметами
        private void HandleSwapItems(int itemIndex1, int itemIndex2)
        {
            inventoryData.SwapItems(itemIndex1, itemIndex2);
        }

        // Обробник події запиту опису предмету
        private void HandleDescriptionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                inventoryUI.ResetSelection();
                return;
            }
            ItemSO item = inventoryItem.item;
            inventoryUI.UpdateDescription(itemIndex, item.ItemImage, item.name, item.Description);
        }

        // Оновлення в кожному кадрі
        public void Update()
        {
            // Перевірка натискання клавіші I або Tab
            if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Tab))
            {
                // Перевірка, чи інтерфейс інвентаря неактивний
                if (inventoryUI.isActiveAndEnabled == false)
                {
                    inventoryUI.Show(); // Відображення інтерфейсу інвентаря
                    foreach (var item in inventoryData.GetCurrentInventoryState())
                    {
                        inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
                    }
                }
                else
                {
                    inventoryUI.Hide(); // Приховання інтерфейсу інвентаря
                }
            }
        }
    }
}
