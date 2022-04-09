using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
public class WeaponZoom : MonoBehaviour
{
    [SerializeField] Camera fpsCamera;
    [SerializeField] RigidbodyFirstPersonController fpsController;
    [SerializeField] float zoomedOutFov = 60f;
    [SerializeField] float zoomedInFov = 20f;
    [SerializeField] float zoomedOutSensitivity = 2f;
    [SerializeField] float zoomedInSensitivity = .3f;

    bool zommedInToggle = false;
	// Start is called before the first frame update

	private void OnDisable()
	{
		ZoomOut();
	}

	// Update is called once per frame
	void Update()
    {
        if (Input.GetMouseButtonDown(1))
		{
            if (zommedInToggle == false)
			{
				ZoomIn();
			}
			else
			{
				ZoomOut();
			}
		}
    }

	private void ZoomOut()
	{
		zommedInToggle = false;
		fpsCamera.fieldOfView = zoomedOutFov;
		fpsController.mouseLook.XSensitivity = zoomedOutSensitivity;
		fpsController.mouseLook.YSensitivity = zoomedOutSensitivity;
	}

	private void ZoomIn()
	{
		zommedInToggle = true;
		fpsCamera.fieldOfView = zoomedInFov;
		fpsController.mouseLook.XSensitivity = zoomedInSensitivity;
		fpsController.mouseLook.YSensitivity = zoomedInSensitivity;
	}
}
