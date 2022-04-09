using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float health = 100f;
	private DeathHandler dh;

	public void Start()
	{
		dh = GetComponent<DeathHandler>();
	}
	public void LoseHealth(float damage)
	{
        health -= damage;
        if (health <= 0)
		{
			dh.HandleDeath();
			Debug.Log("You Dead, my glip glop");
		}
		Debug.Log("Player Health : " + health);
	} 
}
