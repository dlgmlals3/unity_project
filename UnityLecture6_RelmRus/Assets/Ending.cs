using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Ending : MonoBehaviour
{
    // Start is called before the first frame update
    public void goEnding(int balance)
	{
		SceneManager.LoadScene("Ending");
	}
}
