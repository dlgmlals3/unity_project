using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightSystem : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float lightDecay = .1f;
    [SerializeField] float angleDecay = 1f;
    [SerializeField] float minimumAngle = 40f;

    Light myLight;

	private void Start()
	{
        myLight = GetComponent<Light>();
	}

	private void Update()
	{
		DecreaseLightAngle();
		DecreaseLightIntensity();
	}

	public void RestoreLightAngle(float restoreAngle)
	{
		myLight.spotAngle = restoreAngle;
	}

	public void RestoreLightIntensity(float intensityAmount)
	{
		myLight.intensity += intensityAmount;
	}
	private void DecreaseLightAngle()
	{
		if (myLight.spotAngle <= minimumAngle) return;

		myLight.spotAngle -= angleDecay * Time.deltaTime;
	}

	private void DecreaseLightIntensity()
	{
		if (myLight.intensity <= 0) return;
		
		myLight.intensity -= lightDecay * Time.deltaTime;
	}
}
