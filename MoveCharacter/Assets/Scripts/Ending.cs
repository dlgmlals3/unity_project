using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.tag == "Player")
		{
			GetComponent<AudioSource>().Play();
			collision.gameObject.GetComponent<NewMoveManager>().EndTimer();
			//collision.gameObject.GetComponent<MoveManager>().EndTimer();
			collision.gameObject.GetComponent<SimpleSampleCharacterControl>().enabled = false;
		}
	}
}
