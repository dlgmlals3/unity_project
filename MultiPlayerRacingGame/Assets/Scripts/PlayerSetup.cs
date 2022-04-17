using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
public class PlayerSetup : MonoBehaviourPunCallbacks
{
    public Camera PlayerCamera;
    public TextMeshProUGUI PlayerNameText;
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("rc"))
		{
            if (photonView.IsMine) // very important
            {
                // enable carmovement script and camera
                GetComponent<CarMoveMent>().enabled = true;
                GetComponent<LapController>().enabled = true;
                PlayerCamera.enabled = true;
            }
            else
            {
                // player is remote. Disable CarMovement script and Camera
                GetComponent<CarMoveMent>().enabled = false;
                GetComponent<LapController>().enabled = false;
                PlayerCamera.enabled = false;
            }
        }
        else if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("dr"))
		{
            if (photonView.IsMine) // very important
            {
                Debug.Log("11111111111111111111111111");
                // enable carmovement script and camera
                GetComponent<CarMoveMent>().enabled = true;
                GetComponent<CarMoveMent>().controlEnabled = true;
                PlayerCamera.enabled = true;
            }
            else
            {
                Debug.Log("222222222222222222222");
                // player is remote. Disable CarMovement script and Camera
                GetComponent<CarMoveMent>().enabled = false;
                PlayerCamera.enabled = false;
            }
        }
        SetPlayerUI();
    }

    private void SetPlayerUI()
	{
        if (PlayerNameText != null)
		{
            PlayerNameText.text = photonView.Owner.NickName;
            if (photonView.IsMine)
            {
                PlayerNameText.gameObject.SetActive(false);
            }
        }
	}
}
