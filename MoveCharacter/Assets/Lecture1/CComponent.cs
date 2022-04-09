using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CComponent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Object Conrol, gameObject != GameObject

        // GetComponentInParent
/*        
     BoxCollider bx = GetComponentInParent<BoxCollider>();
     bx.isTrigger = true;*/
        

     // Find
     GameObject A = GameObject.Find("A");
     if (A != null)
     {
         A.name = "123";
         A.GetComponent<BoxCollider>().isTrigger = true;
     }

     // FindWithTag
     GameObject B = GameObject.FindWithTag("BTag");
     if (B != null)
     {
         B.SetActive(false);
     }

     // FindObjectOfType
     AComponent ac = FindObjectOfType<AComponent>();
     ac.enabled = false;
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
