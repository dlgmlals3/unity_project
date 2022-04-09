using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
	private void Awake()
	{
		int numMusicPlayers = FindObjectsOfType<MusicPlayer>().Length;
		if (numMusicPlayers > 1)
		{
			Debug.Log("dlgmlals3 first : " + numMusicPlayers.ToString());
			Destroy(gameObject);
		} else
		{
			Debug.Log("dlgmlals3 second : " + numMusicPlayers.ToString());
			DontDestroyOnLoad(gameObject);
		}
	}
}
