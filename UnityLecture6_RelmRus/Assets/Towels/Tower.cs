using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] int cost = 75;
	[SerializeField] int buildDelay = 1;
	void Start()
	{
		Debug.Log("Tower Start");
		StartCoroutine(Build());
	}
	public bool CreateTower(Tower tower, Vector3 position)
	{
        Bank bank = FindObjectOfType<Bank>();

        if (bank == null)
		{
            return false;
		}

        if (bank.CurrentBalance >= cost)
		{
			Debug.Log("Tower Instantiate");
			Instantiate(tower.gameObject, position, Quaternion.identity);
            bank.WithDraw(cost);
            return true;
		}
        return false;
	}
	IEnumerator Build()
	{
		foreach (Transform child in transform)
		{
			child.gameObject.SetActive(false);
			foreach(Transform grandchild in child)
			{
				grandchild.gameObject.SetActive(false);
			}
		}
		
		foreach (Transform child in transform)
		{
			child.gameObject.SetActive(true);
			yield return new WaitForSeconds(buildDelay);
			foreach (Transform grandchild in child)
			{
				grandchild.gameObject.SetActive(true);
			}
		}
	}
}

