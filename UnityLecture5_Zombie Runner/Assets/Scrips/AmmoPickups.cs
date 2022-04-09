using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickups : MonoBehaviour
{
	// Start is called before the first frame update
	[SerializeField] int ammoAmount = 5;
	[SerializeField] AmmoType ammoType;

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("dlgmlals3 OnCollisionEnter");
		if (other.gameObject.tag == "Player")
		{
			Debug.Log("dlgmlals3 pickup");
			FindObjectOfType<Ammo>().IncreaseCurrentAmmo(ammoType, ammoAmount);
			Destroy(gameObject);
		}
	}
}
