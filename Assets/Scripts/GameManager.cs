using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	public int scorePlayer1, scorePlayer2;

	public GameUI gameUI;
	public GameAudio gameAudio;
	// Score player 1 id = 1 Right Score Zone
	// Score player 2 id = 2 Left Score Zone
	
	public int maxScore = 4;
	
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
			gameUI.onStartGame += OnStartGame;
		}
	}
    private void OnDestroy()
    {
        gameUI.onStartGame -= OnStartGame;
    }



    public void OnScoreZoneReached(int id)
	{
		// if (onReset != null)           
		// {							   Same as the line below 
		// 	onReset.Invoke();
		// }


		if(id == 1)
			scorePlayer1++;
		if(id == 2)
			scorePlayer2++;
			
		gameUI.UpdateScore(scorePlayer1,scorePlayer2);
		gameUI.HighLightScore(id);
		CheckWin();
	}

	private void CheckWin()
	{
		int winnerID = scorePlayer1 == maxScore ? 1 : scorePlayer2 == maxScore ? 2 : 0;

		if (winnerID != 0)
		{
			 gameUI.OnGameEnds(winnerID);
			 gameAudio.PlayWinSound();
		}
		else
		{
			onReset?.Invoke();
			gameAudio.PlayScoreSound();
		}
	}
	
	private void OnStartGame()
	{
		scorePlayer1=0;
		scorePlayer2=0;
		gameUI.UpdateScore(scorePlayer1,scorePlayer2);
	}

	
	

	
}