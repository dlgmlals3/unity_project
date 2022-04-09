using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge : MonoBehaviour
{
    // these varriables change our movement speed
    [SerializeField] float xValue = 0f;
    [SerializeField] float yValue = 0f;
    [SerializeField] float zValue = 0f;
    [SerializeField] float speed = 10f;

  // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
      xValue = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
      zValue = Input.GetAxis("Vertical") * Time.deltaTime * speed;
      transform.Translate(xValue, yValue, zValue);
    }
}
