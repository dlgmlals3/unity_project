using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmController : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource au;
    void Start()
    {
        au = GetComponent<AudioSource>();
        au.Play();
    }

}
