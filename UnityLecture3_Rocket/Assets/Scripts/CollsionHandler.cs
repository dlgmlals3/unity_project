using UnityEngine;
using UnityEngine.SceneManagement;

public class CollsionHandler : MonoBehaviour
{

	[SerializeField] private AudioClip success;
	[SerializeField] private AudioClip crash;
	[SerializeField] private AudioClip findTaeri;
	[SerializeField] private float levelLoadDelay = 2f;
	[SerializeField] ParticleSystem successParticles;
	[SerializeField] ParticleSystem crashParticles;

	private int level = 0;
	private AudioSource audioSource;
	
	private bool isTransitioning = false;
	private bool collisionDisabled = false;

	void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	private void Update()
	{
		RespondToDebugKeys();
	}

	private void RespondToDebugKeys()
	{
		if (Input.GetKeyDown(KeyCode.L))
		{
			LoadNextLevel();
		}
		else if (Input.GetKeyDown(KeyCode.C))
		{
			collisionDisabled = !collisionDisabled; // togle collision
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (isTransitioning || collisionDisabled) { return; }
		
		switch (collision.gameObject.tag)
		{
			case "Friendly":
				//StartFriendlySequence();
				//Debug.Log("Friendly");
				break;
			case "Finish":
				StartSuccessSequence();
				Debug.Log("Finish");
				break;
			default:
				Debug.Log("Sorry you blew up");
				StartCrashSequence();
				break;
		}
	}
	void StartFriendlySequence()
	{
		Debug.Log("findTaeri!!!!");
	}
	void StartSuccessSequence()
	{
		isTransitioning = true;
		GetComponent<Movement>().enabled = false;
		audioSource.Stop();
		audioSource.PlayOneShot(success);
		successParticles.Play();
		Debug.Log("StartSuccessSequence!!!!");
		Invoke("LoadNextLevel", levelLoadDelay);
	}
	void StartCrashSequence()
	{
		isTransitioning = true;
		GetComponent<Movement>().enabled = false;
		audioSource.Stop();
		audioSource.PlayOneShot(crash);
		crashParticles.Play();
		Debug.Log("StartCrashSequence!!!!");
		Invoke("ReloadLevel", levelLoadDelay);
	}
	
	void LoadNextLevel()
	{
		int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
		int nextSceneIndex = currentSceneIndex + 1;
		if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
		{
			nextSceneIndex = 0;
		}
		SceneManager.LoadScene(nextSceneIndex);
	}
	private void ReloadLevel()
	{
		int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene(currentSceneIndex);
	}
}
