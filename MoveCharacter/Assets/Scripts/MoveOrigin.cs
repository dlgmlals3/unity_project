using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MoveOrigin : MonoBehaviour
{
    // player move
    [Header("Player Stats")]
    [SerializeField] float xVelocity = 2f;
    [SerializeField] float yForce = 2f;
    [SerializeField] float zVelocity = 2f;
    [SerializeField] float overAir = 1.7f;

    [Space (10f)]
    [SerializeField] TMP_Text score;
    [SerializeField] TMP_Text message;

    private Rigidbody rigidbody;
    private bool isJump = false;
    private float elapsedTime = 0;
    private bool updateTimer = false;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        StartTimer();
    }

    // Update is called once per frame
    // Time.deltaTime; 1frame duration

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * xVelocity;
        float vertical = Input.GetAxis("Vertical") * Time.deltaTime * zVelocity;
        //float zump = Input.GetAxis("Jump") * Time.deltaTime * yVelocity;
        transform.Translate(new Vector3(horizontal, 0, vertical));
        
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

        elapsedTime += Time.deltaTime;

        if (updateTimer)
        {
            score.text = elapsedTime.ToString("F1");
        }
    }
    
    IEnumerator Jump()
	{
        isJump = true;
        rigidbody.AddForce(Vector3.up * yForce, ForceMode.Impulse);
        yield return new WaitForSeconds(overAir);
        isJump = false;
    }
    
    void ResetPosition()
	{
        transform.position = new Vector3(transform.position.x,
            transform.position.y + 20.0f, transform.position.z);
    }

    void StartTimer()
	{
        elapsedTime = 0;
        updateTimer = true;
    }
    public void EndTimer()
	{
        // end music play
        GetComponent<AudioSource>().Play();
        Invoke("showEnding", 1);   
    }

    private void showEnding()
	{
        updateTimer = false;
        message.text = "Success";
        enabled = false;        
    }
}
