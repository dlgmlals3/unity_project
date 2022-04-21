using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lee : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * 4;
        float vertical = Input.GetAxis("Vertical") * Time.deltaTime * 4;
        //float zump = Input.GetAxis("Jump") * Time.deltaTime * yVelocity;
        transform.Translate(new Vector3(horizontal, 0, vertical));

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
