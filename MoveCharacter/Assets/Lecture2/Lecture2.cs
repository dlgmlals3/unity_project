using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Lecture2 : MonoBehaviour
{
    // player move
    [Header("Player Stats")]
    [SerializeField] float xVelocity = 2f;
    [SerializeField] float yForce = 2f;
    [SerializeField] float zVelocity = 2f;

    [Space (10f)]

    private Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    // Time.deltaTime; 1frame duration

    void Update()
    {
/*        float horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * xVelocity;
        float vertical = Input.GetAxis("Vertical") * Time.deltaTime * zVelocity;
        transform.Translate(new Vector3(horizontal, 0, vertical));
        
        if (Input.GetKeyDown(KeyCode.Space))
		{
            rigidbody.AddForce(Vector3.up * yForce, ForceMode.Impulse);
        }*/
    }
	private void FixedUpdate()
	{
		var vertical = Input.GetAxis("Vertical");
		zVelocity = 10f;
		rigidbody.velocity = (transform.forward * vertical) * zVelocity * Time.fixedDeltaTime;
	}
}
