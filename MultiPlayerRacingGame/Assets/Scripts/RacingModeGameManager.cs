using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class RacingModeGameManager : MonoBehaviour
{
    public GameObject[] PlayerPrefabs;
    public Transform[] InstantiatePositions;
    public Text TimeUIText;
    public GameObject[] FinishOrderUIGameObjects;

    public List<GameObject> lapTriggers = new List<GameObject>();

    public static RacingModeGameManager instance = null;

	private void Awake()
	{
        if (instance == null)
		{
            instance = this;
		}
        else if (instance != this)
		{
            Destroy(gameObject);
		}
        DontDestroyOnLoad(gameObject);
    }
	void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
		{
            object playerSelectionNumber;
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerRacingGame.PLAYER_SELECTION_NUMBER, out playerSelectionNumber))
			{
                Debug.Log((int)playerSelectionNumber);
                int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
                Vector3 instantiatePosition = InstantiatePositions[actorNumber - 1].position;
                PhotonNetwork.Instantiate(PlayerPrefabs[(int)playerSelectionNumber].name, instantiatePosition, Quaternion.identity);
            }
        }
        foreach (GameObject gm in FinishOrderUIGameObjects)
		{
            gm.SetActive(false);
		}
    }

}
