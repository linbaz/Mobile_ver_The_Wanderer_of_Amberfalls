using Inventory;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenuButton : MonoBehaviour
{
    public InventoryController inventoryController; // ��������� �� ��� InventoryController

    private void Start()
    {
        // �������� ������ �� ������
        Button btn = GetComponent<Button>();
        // ������ �������� ��䳿 ����������
        btn.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        // ��������� ����������� ���������
        inventoryController.inventoryUI.Show();
    }
}
