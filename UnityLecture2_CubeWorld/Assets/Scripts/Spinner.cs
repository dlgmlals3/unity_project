using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
  // Start is called before the first frame update
  [SerializeField] float xAngle = 0;
  [SerializeField] float yAngle = 0;
  [SerializeField] float zAngle = 0;

  private void Update()
  {
    gameObject.transform.Rotate(xAngle, yAngle, zAngle, Space.Self);
  }
}
