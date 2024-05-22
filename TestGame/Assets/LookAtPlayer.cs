using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private Transform cam;

    void Start()
    {
        cam = Camera.main.transform;
        if (cam == null)
        {
            Debug.LogError("Main camera not found in the scene!");
        }
    }

    void LateUpdate()
    {
        if (cam != null)
        {
            transform.LookAt(cam);
        }
    }
}
