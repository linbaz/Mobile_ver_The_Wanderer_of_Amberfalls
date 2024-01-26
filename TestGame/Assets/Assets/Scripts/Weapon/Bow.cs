using Inventory.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : ToWeapon
{
    public GameObject bullet;
    private float timeFire;
    public float buletSpeed_1;
    public AudioSource sound;
    public Transform handPoint;
    public Transform hand;
    public UIInventoryPage inventory;

    public override void Shoot()
    {
        if(!PauseMenu.GameIsPaused) 
        {     
            if (timeFire <= 0)
            {
                GameObject gObject = Instantiate(bullet, firePoint.position, firePoint.rotation);
                if (gObject != null)
                {
                    gObject.GetComponent<ArrowBullet>().tw = this;
                    gObject.GetComponent<ArrowBullet>().bulletSpeed = buletSpeed_1;
                    shootSound();
                }
                timeFire = fireRate;
            }
        }
    }

    public void shootSound()
    {
        sound.Play();
    }

    public void Start()
    {
        timeFire = fireRate;
        
    }

    private void Update()
    {
        if (handPoint != null && hand != null)
        {
            hand.position = handPoint.position;
            hand.rotation = handPoint.rotation;
        }

        if(!inventory || !inventory.IsInventoryOpen())
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Shoot();
            }
        }

        timeFire -= Time.deltaTime;

    }
}
