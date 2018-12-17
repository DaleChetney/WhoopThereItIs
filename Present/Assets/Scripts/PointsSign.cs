using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PointsSign : MonoSingleton<PointsSign>
{
	public Text pointsText;
	
	void Update()
	{
		int score = GameManagerScript.Instance.Score;
		UpdateScoreText(score);
	}

	public void UpdateScoreText(int score)
	{
		string text = score.ToString();
		pointsText.text = text;
	}
}
