using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("General Setup Settings")]
	[Tooltip("How fast ship moves up and down")] 
	[SerializeField] float controlSpeed = 10f;
	[Tooltip("how fast player moves horizontally")] [SerializeField] float xRange = 10f;
	[Tooltip("how fast player moves vertically")] [SerializeField] float yRange = 7f;

	[Header("Laer gun array")]
	[Tooltip("Add all player lasers here")]
	[SerializeField] GameObject[] lasers;

	[Header("Screen position based tunning")]
	[SerializeField] float positionPitchFactor = -2f;
	[SerializeField] float positionYallFactor = -2f;
	
	[Header("Player input based tunning")]
	[SerializeField] float controlPitchFactor = -10f;
	[SerializeField] float controlRollFactor = -10f;

	private float xThrow;
	private float yThrow;

	protected Joystick joystick;

	private string status;
	private float dragDistance;  //minimum distance for a swipe to be registered
	private Vector3 fp;   //First touch position
	private Vector3 lp;   //Last touch position

	void Start()
	{
		yThrow = 0;
		xThrow = 0;
		dragDistance = Screen.height * 15 / 100;
		status = "";
		joystick = FindObjectOfType<Joystick>();
	}

	// Update is called once per frame
	void Update()
	{
		ProcessJoystick();
		//ProcessKeyBoard();

		ProcessTranslation();
		ProcessRotation();
		ProcessFiring();
	}
	void ProcessJoystick()
	{
		if (joystick)
		{
			//Debug.Log("dlgmlals3 xThrow : " + xThrow + " y : " + yThrow);
			xThrow = joystick.Horizontal * 1.2f;
			yThrow = joystick.Vertical * 1.2f;
		}
	}
	void ProcessKeyBoard()
	{
		xThrow = Input.GetAxis("Horizontal");
		yThrow = Input.GetAxis("Vertical");
	}

	void ProcessTouchInput()
	{
		if (Input.touchCount == 1) // user is touching the screen with a single touch
		{
			Touch touch = Input.GetTouch(0); // get the touch

			if (touch.phase == TouchPhase.Began) //check for the first touch
			{
				fp = touch.position;
				lp = touch.position;
			}
			else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
			{
				lp = touch.position;
			}
			else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
			{
				lp = touch.position;  //last touch position. Ommitted if you use list

				//Check if drag distance is greater than 20% of the screen height
				if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
				{//It's a drag
				 //check if the drag is vertical or horizontal
					if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
					{   //If the horizontal movement is greater than the vertical movement...
						if ((lp.x > fp.x))  //If the movement was to the right)
						{   //Right swipe							
							Debug.Log("dlgmlals3 Right Swipe");
							status = "right";
						}
						else
						{   //Left swipe
							Debug.Log("dlgmlals3 Left Swipe");
							status = "left";
						}
					}
					else
					{   //the vertical movement is greater than the horizontal movement
						if (lp.y > fp.y)  //If the movement was up
						{   //Up swipe
							Debug.Log("dlgmlals3 Up Swipe");
							status = "up";
						}
						else
						{   //Down swipe
							Debug.Log("dlgmlals3 Down Swipe");
							status = "down";
						}
					}
				}
				else
				{   //It's a tap as the drag distance is less than 20% of the screen height
					Debug.Log("Tap");
				}
			}
		}

		float variable = 4f;
		if (status == "right")
		{
			xThrow = Mathf.MoveTowards(xThrow, 1, variable * Time.deltaTime);
		} else if (status == "left")
		{
			xThrow = Mathf.MoveTowards(xThrow, -1, variable * Time.deltaTime);
		}
		if (status == "up")
		{
			yThrow = Mathf.MoveTowards(yThrow, 1, variable * Time.deltaTime);
		}
		else if (status == "down")
		{
			yThrow = Mathf.MoveTowards(yThrow, -1, variable * Time.deltaTime);
		}
	}

	void ProcessFiring()
	{
		if (Input.GetButton("Fire1"))
		{
			ActivateLasers(true);
		}
		else
		{
			ActivateLasers(false);
		}

	}
	void ActivateLasers(bool activate)
	{
		foreach (GameObject laser in lasers)
		{
			var emissionModule = laser.GetComponent<ParticleSystem>().emission;
			emissionModule.enabled = activate;
		}

	}


	void ProcessRotation()
	{
		float pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
		float pitchDueToControlThrow = yThrow * controlPitchFactor;
		float pitch = pitchDueToPosition + pitchDueToControlThrow;

		float yallDueToPosition = transform.localPosition.x * positionYallFactor;
		float yall = yallDueToPosition;

		float rollDueToControlThrow = xThrow * controlRollFactor;
		float roll = rollDueToControlThrow;

		transform.localRotation = Quaternion.Euler(pitch, yall, roll); 
	}

	private void ProcessTranslation()
	{
		//Debug.Log("dlgmlals3 Horizontal rangel : " + xThrow + " vertical : " + yThrow);
		
		float xOffset = xThrow * Time.deltaTime * controlSpeed;
		float newXPos = transform.localPosition.x + xOffset;
		float clampedXPos = Mathf.Clamp(newXPos, -xRange, xRange);

		float yOffset = yThrow * Time.deltaTime * controlSpeed;
		float newYPos = transform.localPosition.y + yOffset;
		float clampedYPos = Mathf.Clamp(newYPos, -yRange, yRange);

		transform.localPosition = new Vector3(clampedXPos, clampedYPos, transform.localPosition.z);
	}
}
