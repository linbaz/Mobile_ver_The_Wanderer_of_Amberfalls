using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class EquippableItemSO : ItemSO, IDestroyableItem, IItemAction
{
    public string ActionName => "Equip";

    [field: SerializeField]

    public AudioClip actionSFX { get; private set; }

    public bool PerformAction(GameObject character)
    {
        AgentWeapon weaponSystem = character.GetComponent<AgentWeapon>();
        if (weaponSystem != null)
        {
            weaponSystem.SetWeapon(this);
            return true;
        }
        return false;
    }
}
