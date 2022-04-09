using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    [SerializeField] GameObject text;

    // Start is called before the first frame update
    void Start()
    {
        text.GetComponent<TextMesh>().text = Score.myScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
