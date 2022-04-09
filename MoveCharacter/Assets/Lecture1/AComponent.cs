using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AComponent : MonoBehaviour
{
    void Start()
    {
        // Component Control
        //BoxCollider bx = gameObject.transform.GetComponent<BoxCollider>();
        BoxCollider bx = GetComponent<BoxCollider>();
        if (bx != null)
        {
            bx.isTrigger = true;
        }
        gameObject.AddComponent<Rigidbody>();
    }
}
