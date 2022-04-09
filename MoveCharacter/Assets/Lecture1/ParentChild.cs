using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentChild : MonoBehaviour
{
    float x, y, z;
    
    // Start is called before the first frame update
    void Start()
    {
        x = transform.localPosition.x;
        y = 0;
        z = 0;
    }

    // Update is called once per frame
    void Update()
    {       
        //x += 0.01f;
        //transform.localPosition = new Vector3(x, y, z);
        //transform.position = new Vector3(x, y, z);

        //Rotation();
        Move();
    }
    void Rotation()
	{
        //transform.RotateAround(transform.parent.position, Vector3.forward, 20 * Time.deltaTime);        
    }
    void Move()
	{
        //transform.Translate(Vector3.right * Time.deltaTime);
        transform.Translate(Vector3.right * Time.deltaTime, Space.World);
    }
}
