using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public ScoreText scoreTextPlayer1, scoreTextPalyer2;
    public GameObject menuObject;  

    public void UpdateScore(int scorePlayer1, int scorePlayer2)
	{
		scoreTextPlayer1.SetScore(scorePlayer1);
		scoreTextPalyer2.SetScore(scorePlayer2);
	}

    public void HighLightScore(int id)
	{
		if (id == 1)
		{
			scoreTextPlayer1.Highlight();
		}
		else
		{
			scoreTextPalyer2.Highlight();
		}
	}

    public void OnStartGameButtonClicked()
    {
            menuObject.SetActive(false);
    }

}

    
