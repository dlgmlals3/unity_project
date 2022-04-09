using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float rotationThrust = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] ParticleSystem mainEngineParticle;
    [SerializeField] ParticleSystem leftThrusterParticle;
    [SerializeField] ParticleSystem rightThrusterParticle;

    private Rigidbody rb;
    private AudioSource au;

    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered
    void Start()
    {
        dragDistance = Screen.height * 15 / 100;
        rb = GetComponent<Rigidbody>();
        au = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            ProcessTouchEvent();
        } else
		{
            ProcessThrust();
            ProcessRotation();
        }
    }
    void ProcessTouchEvent()
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
                lp = touch.position;  //last touch position. Ommitted if you use list

                //Check if drag distance is greater than 20% of the screen height
                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {//It's a drag
                 //check if the drag is vertical or horizontal
                    if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                    {   //If the horizontal movement is greater than the vertical movement...
                        if ((lp.x > fp.x))  //If the movement was to the right)
                        {   //Right swipe
                            RotateRight();
                        }
                        else
                        {   //Left swipe
                            RotateLeft();
                        }
                        StopRotate();
                    }
                }
                else
                {   //It's a tap as the drag distance is less than 20% of the screen height
                    Debug.Log("dlgmlals3 Tap");
                }

            }
            else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
            {
                StopThrusting();
                StopRotate();
            } else if (touch.phase == TouchPhase.Stationary)
			{                
                StartThrusting();
            }
        }
    }


    private void ProcessThrust()
	{
        if (Input.GetKey(KeyCode.Space))
		{            
            StartThrusting();
		}
		else
		{
            StopThrusting();
		}
	}
	
	private void ProcessRotation()
    {         
        if (Input.GetKey(KeyCode.A))
		{
            Debug.Log("dlgmlals3 ProcessRotation A");
            RotateLeft();
		}
		else if (Input.GetKey(KeyCode.D))
		{
            Debug.Log("dlgmlals3 ProcessRotation D");
            RotateRight();
		}
		else
		{
            Debug.Log("dlgmlals3 Stop");
            StopRotate();
		}
	}
	private void StopThrusting()
	{
		au.Stop();
		mainEngineParticle.Stop();
	}
	private void StartThrusting()
	{
        
		rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
		if (!au.isPlaying)
		{
			au.PlayOneShot(mainEngine);
		}
		if (!mainEngineParticle.isPlaying)
		{
			mainEngineParticle.Play();
		}
	}
	private void StopRotate()
	{
		leftThrusterParticle.Stop();
		rightThrusterParticle.Stop();
	}

	private void RotateRight()
	{
		ApplyRotation(-rotationThrust);
		if (!leftThrusterParticle.isPlaying)
		{
			leftThrusterParticle.Play();
		}
	}

	private void RotateLeft()
	{
		ApplyRotation(rotationThrust);
		if (!rightThrusterParticle.isPlaying)
		{
			rightThrusterParticle.Play();
		}
	}

	public void ApplyRotation(float rotationThisFrame)
	{
        // 다른 물리물체랑 부딪혔을때 constraint를 풀어줘야 부딪치고 난 후의 액션이 제대로 동작한다 
        rb.freezeRotation = true; // freezing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false; // unfreezing rotation so the physics system can take over
    }
}
