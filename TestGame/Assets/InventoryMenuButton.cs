using Inventory;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenuButton : MonoBehaviour
{
    public InventoryController inventoryController; // Посилання на ваш InventoryController

    private void Start()
    {
        // Отримати доступ до кнопки
        Button btn = GetComponent<Button>();
        // Додати обробник події натискання
        btn.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        // Викликати ініціалізацію інвентаря
        inventoryController.inventoryUI.Show();
    }
}
