using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 curPosition;
    private Vector3 curRotation;
    private float speed;
    
    void Start()
    {
        curPosition = transform.position;
        curRotation = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        float period = 2; // 1 seconds
        float cycles = Time.time / period;
        const float rad = Mathf.PI * 2;
        float wave = Mathf.Sin(cycles * rad); // -1 ~ 1
        float factor = (wave + 1f) / 2f; // 0 ~ 1
        
        Vector3 distance = new Vector3(5, 0, 0);
        transform.position = curPosition + (distance * factor);
        
        Vector3 rotation = new Vector3(45, 0, 0);
        transform.rotation = Quaternion.Euler(curRotation + (rotation * factor));

    }
}
