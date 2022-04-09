using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public static class Score
{
	public static int myScore;
}

public class Bank : MonoBehaviour
{
	[SerializeField] int startingBalance = 150;
	[SerializeField] int currentBalance;
	[SerializeField] TextMeshProUGUI displayBalance;
	public int CurrentBalance { get { return currentBalance;  } }

	private void Awake()
	{
		currentBalance = startingBalance;
		UpdateDisplay();
	}

	void UpdateDisplay()
	{
		displayBalance.text = "Gold: " + currentBalance;
		Score.myScore = currentBalance;
	}
	public void Deposit(int amount)
	{
		currentBalance += Mathf.Abs(amount);
		UpdateDisplay();
	}

	public void WithDraw(int amount)
	{
		currentBalance -= Mathf.Abs(amount);

		if (currentBalance < 0)
		{
			// Lose the game
			Scene currentScene = SceneManager.GetActiveScene();
			SceneManager.LoadScene(currentScene.buildIndex);
		}
		UpdateDisplay();
	}

}
