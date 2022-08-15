using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeRotateor : MonoBehaviour
{
    [SerializeField]
    private Vector3 speed = Vector3.zero;

    // Update is called once per frame
    void Update() => transform.Rotate(speed* Time.deltaTime, Space.World);  
    
}
