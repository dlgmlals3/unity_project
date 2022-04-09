using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public static class Score
{
    public static int myScore;
}

public class ScoreBoard : MonoBehaviour
{
    int score;
    TMP_Text scoreText;

    void Start()
	{
		scoreText = GetComponent<TMP_Text>();
        scoreText.text = "Start";
    }

	public void IncreaseScore(int amountToIncrease)
	{
        score += amountToIncrease;
	}

    // Update is called once per frame
    void Update()
    {
        scoreText.text = score.ToString();
        Score.myScore = score;
    }
}
