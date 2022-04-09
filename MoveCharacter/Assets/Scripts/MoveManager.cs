using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MoveManager : MonoBehaviour
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
    private Vector3 startPos;
    private bool isJump = false;
    private float elapsedTime = 0;
    private bool updateTimer = false;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        startPos = transform.position;
        StartTimer();
    }

    // Update is called once per frame
    // Time.deltaTime; 1frame duration

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * xVelocity;
        float vertical = Input.GetAxis("Vertical") * Time.deltaTime * zVelocity;
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

    void StartTimer()
	{
        elapsedTime = 0;
        updateTimer = true;
    }
    public void EndTimer()
	{
        Invoke("showEnding", 1);   
    }

    private void showEnding()
	{
        updateTimer = false;
        message.text = "Success";
        enabled = false;        
    }
}
