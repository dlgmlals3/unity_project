
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BComponent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		// 자식만 찾을경우
		/*foreach (Transform t in transform)
		{
            Debug.Log("BComponent : " + t.gameObject.name);
            t.GetComponent<Rigidbody>().isKinematic = true;
            t.GetComponent<Rigidbody>().useGravity = false;
        }*/
		// search rigidbody in child component

		// 자신포함
		/*Rigidbody rb = GetComponentInChildren<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }*/

		// search all rigidbody in child component
        // 자신
		Rigidbody[] rb2 = GetComponentsInChildren<Rigidbody>();
		if (rb2[0] != null)
		{
			rb2[0].useGravity = false;
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
