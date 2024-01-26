using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NefreteeBow : Bow
{
    public override void Shoot()
    {
        
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Shoot();
        }
    }
}
