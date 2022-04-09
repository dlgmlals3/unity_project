using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropper : MonoBehaviour
{
    [SerializeField] 
    private float waitTime = 3.0f;
    private MeshRenderer renderer;
  private Rigidbody rigidbody;
  // Start is called before the first frame update
  void Start()
    {
      renderer = GetComponent<MeshRenderer>();
      rigidbody = GetComponent<Rigidbody>();
      renderer.enabled = false;
      rigidbody.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
      if (Time.time > waitTime)
      {
        renderer.enabled = true;
        rigidbody.useGravity = true;
        // Debug.Log("3 seconds has elapsed");
      }
    }
}
