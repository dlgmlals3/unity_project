using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField] GameObject deathFX;
	[SerializeField] GameObject hitVFX;
	
	[SerializeField] int scorePerHit = 15;
	[SerializeField] int hitPoints = 4;

	private GameObject parentGameObject;
	private ScoreBoard scoreBoard;
	
	
	void Start()
	{
		scoreBoard = FindObjectOfType<ScoreBoard>();
		parentGameObject = GameObject.FindWithTag("SpawnAtRuntime");
		AddRigidBody();
	}

	private void AddRigidBody()
	{
		Rigidbody rb = gameObject.AddComponent<Rigidbody>();
		rb.useGravity = false;
	}
	private void OnParticleCollision(GameObject other)
	{
		Debug.Log("OnParticleCollision reminaning energy : " + hitPoints);
		ProcessHit();
		
		if (hitPoints < 1)
		{
			KillEnemy(other);
		}
	}
	private void ProcessHit()
	{
		GameObject vfx = Instantiate(hitVFX, transform.position, Quaternion.identity);
		Debug.Log("ProcessHit");
		vfx.transform.parent = parentGameObject.transform;
		hitPoints--;
	}

	private void KillEnemy(GameObject other)
	{
		scoreBoard.IncreaseScore(scorePerHit);
		GameObject fx = Instantiate(deathFX, transform.position, Quaternion.identity);
		fx.transform.parent = parentGameObject.transform; 
		Destroy(gameObject);
	}


}
