using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coroutine : MonoBehaviour
{
    private IEnumerator coroutine;
    private Rigidbody rb;
    void Start()
    {
        print("Coroutine 1 " + Time.time + " seconds");
        coroutine = WaitAndPrint(2.0f);
        print("Coroutine 2 " + Time.time + " seconds");
        StartCoroutine(coroutine);
        print("Coroutine 3 " + Time.time + " seconds");
    }

    private IEnumerator WaitAndPrint(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        print("Coroutine ended: " + Time.time + " seconds");
    }

    // jump
    private IEnumerator Jump(float waitTime)
    {
        rb.AddForce(new Vector3(0f, 1f, 0f));
        yield return new WaitForSeconds(waitTime);     
    }
}
