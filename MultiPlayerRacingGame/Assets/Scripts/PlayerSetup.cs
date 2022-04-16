using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    public Camera PlayerCamera;
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine) // very important
        {
            // enable carmovement script and camera
            GetComponent<CarMoveMent>().enabled = true;
            PlayerCamera.enabled = true;
		}   
        else
		{
            // player is remote. Disable CarMovement script and Camera
            GetComponent<CarMoveMent>().enabled = false;
            PlayerCamera.enabled = false;
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
