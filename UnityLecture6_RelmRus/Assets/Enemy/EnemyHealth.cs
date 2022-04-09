using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// EnemyHealth ������Ʈ ����ϸ� �ڵ����� Enemy�� �������.
[RequireComponent(typeof(Enemy))]

public class EnemyHealth : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int maxHitPoints = 5;
    [Tooltip("Add amount to maxHitpoints when enemy dies.")]
    [SerializeField] int difficultyRamp = 1;

    int currentHitPoints;
    Enemy enemy;

	void Start()
	{
        enemy = GetComponent<Enemy>();
	}

	void OnEnable()
    {
        currentHitPoints = maxHitPoints;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnParticleCollision(GameObject other)
	{
        processHIt();
	}

	private void processHIt()
	{
        currentHitPoints--;
        if (currentHitPoints < 0)
		{
            gameObject.SetActive(false);
            maxHitPoints += difficultyRamp;
            enemy.RewardGold();
        }
	}
}
