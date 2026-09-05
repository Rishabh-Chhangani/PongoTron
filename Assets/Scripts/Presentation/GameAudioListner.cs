using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudioListner : MonoBehaviour
{
    private GameAudio gameAudio;

    private void Awake()
    {
        // Automatically grab the existing GameAudio component on this object
        gameAudio = GetComponent<GameAudio>();
    }

    private void OnEnable()
    {
        // Subscribe to the GameManager's broadcasts
        GameManager.OnScoreUpdated += HandleScoreSound;
        GameManager.OnGameWon += HandleWinSound;
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        GameManager.OnScoreUpdated -= HandleScoreSound;
        GameManager.OnGameWon -= HandleWinSound;
    }

    private void HandleScoreSound(int playerIndex, int newScore)
    {
        gameAudio.PlayScoreSound();
    }

    private void HandleWinSound(int winningPlayerIndex)
    {
        gameAudio.PlayWinSound();
    }
}
