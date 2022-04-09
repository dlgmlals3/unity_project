using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float hitPoints = 100f;
    [SerializeField] Canvas Ending;
    // create a public method which reduces hitpoints by the amount of damage
    private bool isDead = false;
	void Start()
	{
        Ending.enabled = false;
	}
	public bool IsDead()
	{
        return isDead;
	}

	public void TakeDamage(float damage)
	{
        // GetComponent<EnemyAI>().OnDamageTaken();
        BroadcastMessage("OnDamageTaken"); // simulataneously call which name is same...

        hitPoints -= damage;
        if (hitPoints <= 0)
		{
            Die();
		}
	}

    public void Die()
	{
        if (!isDead)
        {
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            if (tag == "King")
			{
                Ending.enabled = true;
            }
            Debug.Log("DIE ????????????");
            GetComponent<AudioSource>().Play();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
