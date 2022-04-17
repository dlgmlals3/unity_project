using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class LapController : MonoBehaviourPun
{
    private List<GameObject> LabTriggers = new List<GameObject>();
    
    // RPC 만으로 부족 raise 쓰자
    public enum RaiseEventCode
	{
        WhoFinishedEventCode = 0
	}
    private int finishOrder = 0;
    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject lapTrigger in RacingModeGameManager.instance.lapTriggers)
		{
            LabTriggers.Add(lapTrigger);
		}
    }

	private void OnEnable()
	{
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }
	private void OnDisable()
	{
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }
	void OnEvent(EventData photonEvent)
	{
        if (photonEvent.Code == (byte)RaiseEventCode.WhoFinishedEventCode) 
        {
            object[] data = (object[])photonEvent.CustomData;
            string nickNameOfFinishedPlayer = (string)data[0];
            finishOrder = (int)data[1];
            int viewId = (int)data[2];
            Debug.Log("dlgmlals3 OnEvent " + finishOrder + " viewID : " + viewId + " " + "name : " + nickNameOfFinishedPlayer);

            GameObject orderUITextGameObject = RacingModeGameManager.instance.FinishOrderUIGameObjects[finishOrder - 1];
            orderUITextGameObject.SetActive(true);
            
            if (viewId == photonView.ViewID)
			{
                orderUITextGameObject.GetComponent<Text>().text = finishOrder + ". " + nickNameOfFinishedPlayer + " ( you )";
                orderUITextGameObject.GetComponent<Text>().color = Color.red;
            } else
			{
                orderUITextGameObject.GetComponent<Text>().text = finishOrder + ". " + nickNameOfFinishedPlayer;
            }
            
        }
	}


	private void OnTriggerEnter(Collider other)
	{
        
        if (LabTriggers.Contains(other.gameObject))
		{
            int indexOfTrigger = LabTriggers.IndexOf(other.gameObject);
            //LabTriggers[indexOfTrigger].SetActive(false);
            if (other.name == "FinishTrigger")
			{
                // game is finished
                GameFinished();
			}
        }
	}

    void GameFinished()
	{
        GetComponent<PlayerSetup>().PlayerCamera.transform.parent = null;
        GetComponent<CarMoveMent>().enabled = false;
        
        finishOrder += 1;
        string nickName = photonView.Owner.NickName;
        int viewId = photonView.ViewID;
        
        object[] data = new object[] { nickName, finishOrder, viewId};
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All,
            CachingOption = EventCaching.AddToRoomCache
        };
        SendOptions sendOptions = new SendOptions
        {
            Reliability = true
        };
        Debug.Log("dlgmlals3 send raise order" + finishOrder + " view : "+ viewId +" " + nickName);
        PhotonNetwork.RaiseEvent((byte)RaiseEventCode.WhoFinishedEventCode, data, raiseEventOptions, sendOptions);
	}
}
