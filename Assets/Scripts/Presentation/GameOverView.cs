using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverView : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI winnerText;

    private void OnEnable()
    {
        // Listen for the win condition
        GameManager.OnGameWon += HandleGameWon;
    }

    private void OnDisable()
    {
        GameManager.OnGameWon -= HandleGameWon;
    }

    private void HandleGameWon(int winningPlayerIndex)
    {

        winnerText.text = $"PLAYER {winningPlayerIndex} WINS!";

        
        gameOverPanel.SetActive(true);

        
        Time.timeScale = 0f;
    }
}

