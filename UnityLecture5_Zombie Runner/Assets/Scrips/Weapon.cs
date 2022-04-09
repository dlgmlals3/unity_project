using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] Camera FPCamera;
    [SerializeField] float range = 100f;
    [SerializeField] float damage = 30f;
	[SerializeField] ParticleSystem muzzleFlash;
	[SerializeField] GameObject hitEffect;
	[SerializeField] Ammo ammoSlot;
	[SerializeField] AmmoType ammoType;
	[SerializeField] float timeBetweenShots = 0.5f;
	[SerializeField] TextMeshProUGUI ammoText;

	bool canShoot = true;

	private void OnEnable()
	{
		canShoot = true;
	}

	// Start is called before the first frame update
	void Update()
    {
		DisplayAmmo();
		// dlgmlals3

        if (Input.GetMouseButton(0) && canShoot == true) {
			StartCoroutine(Shoot());
        }

    }
	// dlgmlals3
	public void PlayerShoot()
	{
		if (canShoot == true)
		{
			StartCoroutine(Shoot());
		}
	}

	private void DisplayAmmo()
	{
		int currentAmmo = ammoSlot.GetCurrentAmmo(ammoType);
		ammoText.text = currentAmmo.ToString();
	}

	IEnumerator Shoot()
	{
		canShoot = false;
		if (ammoSlot.GetCurrentAmmo(ammoType) > 0)
		{
			PlayMuzzleFlash();
			ProcessRayCast();
			ammoSlot.ReduceCurrentAmmo(ammoType);
			GetComponent<AudioSource>().Play();
		}
		yield return new WaitForSeconds(timeBetweenShots);
		canShoot = true;
	}
	private void PlayMuzzleFlash()
	{
		muzzleFlash.Play();
	}

	private void ProcessRayCast()
	{
		RaycastHit hit;
		if (Physics.Raycast(FPCamera.transform.position, FPCamera.transform.forward, out hit, range))
		{
			// add some hit effect for visual players
			CreateHitImpact(hit);
			EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
			if (target == null) return;
			target.TakeDamage(damage);
			// call a method on EnemyHealth that decreases the enemy's health
		}
		else
		{
			return;
		}
	}

	private void CreateHitImpact(RaycastHit hit)
	{
		Vector3 newHit = new Vector3(hit.point.x, hit.point.y-0.3f, hit.point.z); 
		GameObject impact = Instantiate(hitEffect, newHit, Quaternion.LookRotation(hit.normal));
		Destroy(impact, 0.1f);
	}
}
