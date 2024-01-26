using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToWeapon : MonoBehaviour
{
    public int damage;
    public float fireRate;
    public Transform firePoint;

    public virtual void Shoot()
    {

    }

    public virtual void Reload()
    {

    }

    public int getDamage()
    {
        return damage;
    }

}
