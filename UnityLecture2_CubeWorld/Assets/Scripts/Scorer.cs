using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scorer : MonoBehaviour
{
  private int hits = 0;
  private void OnCollisionEnter(Collision collision)
  {
    Debug.Log("you've bumped into many times." + hits);
    if (collision.gameObject.tag != "Hit") { 
      hits++;
    }
  }
}
