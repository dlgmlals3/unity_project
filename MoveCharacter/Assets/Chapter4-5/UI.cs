using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UI : MonoBehaviour
{
    // 1
    //public GameObject textMeshObj;
    //[SerializeField] private GameObject tezxt;
    // Start is called before the first frame update
    void Start()
    {
        GameObject textMeshObj = GameObject.Find("textMesh");

        Component[] cp = textMeshObj.GetComponents(typeof(Component));
        foreach (Component component in cp)
        {
            Debug.Log(" " + component.GetType());
        }

        GetComponentInChildren<TextMeshProUGUI>().text = "gogogogo";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
