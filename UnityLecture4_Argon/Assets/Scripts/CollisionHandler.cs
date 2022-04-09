using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
	[SerializeField] float loadDelay = 1f;
	[SerializeField] ParticleSystem crashVFX;

	private void OnTriggerEnter(Collider other)
	{
		StartCrashSequence();
	}
	private void OnTriggerExit(Collider other)
	{

	}
	public void StartCrashSequence()
	{
		crashVFX.Play();

		MeshRenderer[] renderer = GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer r in renderer)
		{
			r.enabled = false;
		}

		GetComponent<BoxCollider>().enabled = false;
		GetComponent<PlayerControls>().enabled = false;
		Invoke("ReloadLevel", loadDelay);
	}


	private void ReloadLevel() {
		int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene(currentSceneIndex);
	}

}
