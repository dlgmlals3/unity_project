using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] Transform weapon;
    [SerializeField] float range = 15;
    [SerializeField] ParticleSystem projectileParticles;
    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<EnemyMover>().transform;
    }

    void Update()
    {
        FindClosestTarget();
        AimWepon();        
    }

	private void FindClosestTarget()
	{
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Transform closestTarget = null;
		float maxDistance = Mathf.Infinity;

        foreach (Enemy enemy in enemies)
		{
            float targetDistance = Vector3.Distance(transform.position, enemy.transform.position);
            if (maxDistance > targetDistance)
			{
                maxDistance = targetDistance;
                closestTarget = enemy.transform;
            }
		}

        target = closestTarget;
	}

	private void AimWepon()
	{
        float targetDistance = Vector3.Distance(transform.position, target.position);
        weapon.LookAt(target);	
        if (targetDistance < range)
		{
            Attack(true);
		} else
		{
            Attack(false);
		}
	}


    void Attack(bool isActive) 
    {
        var emissionModule = projectileParticles.emission;
        emissionModule.enabled = isActive;
    }
}
