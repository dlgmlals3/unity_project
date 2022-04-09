using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Move : MonoBehaviour
{
    // player move
    [Header("Player Stats")]
    [SerializeField] float xVelocity = 2f;
    [SerializeField] float yForce = 2f;
    [SerializeField] float zVelocity = 2f;
    [SerializeField] float overAir = 1.7f;

    [Space(10f)]

    [SerializeField] TMP_Text score;
    [SerializeField] TMP_Text message;

    private Rigidbody rigidbody;
    private Vector3 startPos;
    private bool isJump = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        startPos = transform.position;

    }

    // Update is called once per frame
    // Time.deltaTime; 1frame duration
    void FixedUpdate()
	{
        // phsycis
	}
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * xVelocity;
        float vertical = Input.GetAxis("Vertical") * Time.deltaTime * zVelocity;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isJump)
            {
                StartCoroutine(Jump());
            }
        }

        if (transform.position.y < -30f)
        {
            ResetPosition();
        }

        transform.Translate(new Vector3(horizontal, 0, vertical));
    }
    void ResetPosition()
    {
        transform.position = new Vector3(startPos.x, startPos.y + 10.0f, startPos.z);
    }

    IEnumerator Jump()
    {
        isJump = true;
        rigidbody.AddForce(Vector3.up * yForce, ForceMode.Impulse);
        yield return new WaitForSeconds(overAir);
        isJump = false;
    }

	private void OnCollisionEnter(Collision collision)
	{
        Debug.Log("OnCollisionEnter : " + collision.gameObject.name);
    }
	private void OnTriggerEnter(Collider other)
	{
        Debug.Log("OnTriggerEnter : " + other.gameObject.name);
        transform.eulerAngles = new Vector3(30f, 40f, 0);
    }
}
