using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class NewMoveManager : MonoBehaviour
{
    [SerializeField] TMP_Text score;
    [SerializeField] TMP_Text message;

    private Rigidbody rigidbody;
    private Vector3 startPos;
    private float elapsedTime = 0;
    private bool updateTimer = false;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        StartTimer();        
    }

    // Update is called once per frame
    // Time.deltaTime; 1frame duration

    void Update()
    {
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
