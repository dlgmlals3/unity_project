using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHit : MonoBehaviour
{
  private MeshRenderer re;
  
  void Start()
  {
    re = GetComponent<MeshRenderer>();
  }
  private void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.tag == "Player")
    {
      // Debug.Log("OnCollisionEnter " + collision.gameObject.name);
      re.material.color = Color.blue;
      gameObject.tag = "Hit";
    }    
  }
}
