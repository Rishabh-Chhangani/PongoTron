using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	public int scorePlayer1, scorePlayer2;
	// Score player 1 id = 1 Right Score Zone
	// Score player 2 id = 2 Left Score Zone
	
	public ScoreText scoreTextLeft, scoreTextRight;
	public Action onReset;	

	public void Awake()
	{
		if(instance)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
		}
	}



	public void OnScoreZoneReached(int id)
	{
		// if (onReset != null)           
		// {							   Same as the line below 
		// 	onReset.Invoke();
		// }

		onReset?.Invoke();

		if(id == 1)
			scorePlayer1++;
		if(id == 2)
			scorePlayer2++;
			
		UpdateScore();
	}
	
	private void UpdateScore()
	{
		scoreTextLeft.SetScore(scorePlayer1);
		scoreTextRight.SetScore(scorePlayer2);
	}
	
}