using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{	
	[SerializeField] float damage = 40f;

	PlayerHealth target;
	void Start()
	{
		target = FindObjectOfType<PlayerHealth>();
	}

	public void AttackHitEvent()
	{
		if (target == null) return;
		target.LoseHealth(damage);
		Debug.Log("dlgmlals3 " + name + " attack " + target.name);
		target.GetComponent<DisplayDamage>().ShowDamageImpact();
	}

	public void OnDamageTaken()
	{
		Debug.Log(name + " I also know that we took damage");		
	}
}
