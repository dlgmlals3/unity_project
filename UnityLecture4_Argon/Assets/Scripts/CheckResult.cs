using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckResult : MonoBehaviour
{
    private int count = 25;
    CollisionHandler col;
    // Start is called before the first frame update
    void Start()
    {
        col = FindObjectOfType<CollisionHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnParticleCollision(GameObject other)
    {
        count--;
        if (count > 0) return;
        if (tag == "Answer")
        {
            Invoke("ReloadLevel", 2f);
        }
        else
        {
            col.StartCrashSequence();
        }
    }
    private void ReloadLevel()
    {
        SceneManager.LoadScene("Ending");
    }
}
