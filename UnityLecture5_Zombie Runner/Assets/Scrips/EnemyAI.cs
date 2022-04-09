using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] float chaseRange = 5f;
    [SerializeField] float turnSpeed = 5f;

    Transform target;

    NavMeshAgent navMeshAgent;
    float distanceToTarget = Mathf.Infinity;
    bool isProvoked = false;

    EnemyHealth health;
    // Start is called before the first frame update

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        health = GetComponent<EnemyHealth>();
        target = FindObjectOfType<PlayerHealth>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (health.IsDead())
		{
            enabled = false;
            navMeshAgent.enabled = false;
		}
        distanceToTarget = Vector3.Distance(target.position, transform.position);
        if (isProvoked)
		{
            EngageTarget();
        }
         else if (distanceToTarget <= chaseRange)
		{
            isProvoked = true;
		}
    }

    public void OnDamageTaken()
	{
        isProvoked = true;
	}

    private void EngageTarget()
	{
        faceTarget();
        if (distanceToTarget >= navMeshAgent.stoppingDistance)
        {
            //Debug.Log("EngageTarget " + distanceToTarget + " stop : " + navMeshAgent.stoppingDistance);
            ChaseTarget();
        }
        else if (distanceToTarget < navMeshAgent.stoppingDistance)
        {
            AttackTarget();
        }
	}

    private void ChaseTarget()
    {
        // move
        GetComponent<Animator>().SetBool("attack", false);
        GetComponent<Animator>().SetTrigger("move");
        navMeshAgent.SetDestination(target.position);
    }

    private void AttackTarget()
	{
        GetComponent<Animator>().SetBool("attack", true);
	}

    private void faceTarget() 
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        // transform.rotation = where the target is , we need to rotate at a certation speed
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
