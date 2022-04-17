using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BulletScript : MonoBehaviour
{
    private float bulletDamage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void OnTriggerEnter(Collider other)
	{
		Destroy(gameObject);
        if (other.gameObject.CompareTag("Player"))
		{
            if (other.gameObject.GetComponent<PhotonView>().IsMine)
			{
                other.gameObject.GetComponent<PhotonView>().RPC("DoDamage", RpcTarget.AllBuffered, bulletDamage);
            }
		}
	}

	public void Initialize(Vector3 _direction, float speed, float damage)
	{
        bulletDamage = damage;
        transform.forward = _direction;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = _direction * speed;
	}
}
