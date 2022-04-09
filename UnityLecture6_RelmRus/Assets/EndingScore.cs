using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndingScore : MonoBehaviour
{
    [SerializeField] TextMeshPro textMeshPro;
    // Start is called before the first frame update
    void Start()
    {
        textMeshPro.text = Score.myScore.ToString();
    }
}
