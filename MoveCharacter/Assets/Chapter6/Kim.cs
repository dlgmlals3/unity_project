using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kim : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	private void OnCollisionEnter(Collision collision)
	{
        Debug.Log("Im " + gameObject.name + " collision : " + collision.gameObject.name);
    }
	private void OnTriggerEnter(Collider other)
	{
        Debug.Log("Im " + gameObject.name + " trigger : " + other.gameObject.name);
    }
}
