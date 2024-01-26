using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentWeapon : MonoBehaviour
{
    [SerializeField]
    private EquippableItemSO weapon;

    [SerializeField]
    private InventorySO inventoryData;


    public void SetWeapon(EquippableItemSO weaponItemSO)
    {
        if (weapon != null)
        {
            inventoryData.AddItem(weapon, 1);
        }

        this.weapon = weaponItemSO;
    }
}
