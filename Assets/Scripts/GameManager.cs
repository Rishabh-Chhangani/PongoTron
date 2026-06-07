using UnityEngine;

public class GameManager : MonoBehaviour
{
	public int scorePlayer1, scorePlayer2;
	// Score player 1 id = 1 Right Score Zone
	// Score player 2 id = 2 Left Score Zone
	
	public ScoreText scoreTextLeft, scoreTextRight;
	public void OnScoreZoneReached(int id)
	{
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