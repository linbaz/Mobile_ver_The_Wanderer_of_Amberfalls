using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class WeaponSwitchTests
{
    [Test]
    public void WeaponSwitch_SelectWeapon_SwitchToFirstWeapon()
    {
        var gameObject = new GameObject();
        var weaponSwitch = gameObject.AddComponent<WeaponSwitch>();
        var sword1 = new GameObject().AddComponent<Sword>();
        sword1.gameObject.transform.SetParent(gameObject.transform);

        weaponSwitch.selectedWeapon = 1;
        weaponSwitch.swords = new Sword[] { sword1 };

        weaponSwitch.SelectWeapon();

        Assert.IsFalse(sword1.gameObject.activeSelf);
    }

    [Test]
    public void WeaponSwitch_SelectWeapon_SwitchToSecondWeapon()
    {
        var gameObject = new GameObject();
        var weaponSwitch = gameObject.AddComponent<WeaponSwitch>();
        var sword1 = new GameObject().AddComponent<Sword>();
        var sword2 = new GameObject().AddComponent<Sword>();

        sword1.gameObject.transform.SetParent(gameObject.transform);
        sword2.gameObject.transform.SetParent(gameObject.transform);

        weaponSwitch.selectedWeapon = 1;
        weaponSwitch.swords = new Sword[] { sword1, sword2 };

        weaponSwitch.SelectWeapon();

        Assert.IsFalse(sword1.gameObject.activeSelf);
        Assert.IsTrue(sword2.gameObject.activeSelf);
    }

    [Test]
    public void WeaponSwitch_Update_SelectedWeaponChanged()
    {
        var gameObject = new GameObject();
        var weaponSwitch = gameObject.AddComponent<WeaponSwitch>();
        var sword1 = new GameObject().AddComponent<Sword>();
        var sword2 = new GameObject().AddComponent<Sword>();

        sword1.gameObject.transform.SetParent(gameObject.transform);
        sword2.gameObject.transform.SetParent(gameObject.transform);

        weaponSwitch.selectedWeapon = 0;
        weaponSwitch.swords = new Sword[] { sword1, sword2 };

        weaponSwitch.Update();

        Assert.IsTrue(sword1.gameObject.activeSelf);
        Assert.IsTrue(sword2.gameObject.activeSelf);
    }
}
