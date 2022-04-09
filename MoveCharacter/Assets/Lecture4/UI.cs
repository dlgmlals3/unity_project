using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
/*        GameObject obj = GameObject.Find("textMesh");
        Component[] cp = obj.GetComponents(typeof(Component));
        foreach(Component component in cp)
		{
            Debug.Log(" " + component.GetType());
		}*/
        GetComponentInChildren<TextMeshProUGUI>().text = "gogogogo";
        // TMPro.TextMeshProUGUI
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
