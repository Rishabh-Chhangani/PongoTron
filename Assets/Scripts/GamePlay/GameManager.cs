using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    // --BroadCasters--
    public static event Action<int, int> OnScoreUpdated;
    public static event Action<int> OnGameWon;
    public static event Action OnRoundReset;

    //--Core Logic Variables--
    public Ball ball;
    public Paddle playerPaddle;
    public Paddle computerPaddle;
    public int pointsToWin = 5;

    private int _playerScore;
    private int _computerScore;



    private void Awake()
    {
       
        if(gameObject == null)
        {
            Debug.LogError("GameManager GameObject is null!");
        }

    }

    void Start()
    {
        // SINGLETON PATTERN - Destroy duplicates
        int numGameManagers = FindObjectsOfType<GameManager>().Length;
        if (numGameManagers > 1)
        {
            Destroy(gameObject);
            return;
        }


        _playerScore = 0;
        _computerScore = 0;

        

    }

    public void PlayerScore()
    {
        _playerScore++;
        OnScoreUpdated?.Invoke(1, _playerScore);
        
        Debug.Log($"Player Score:{_playerScore}, Win points {pointsToWin} ");

        if (_playerScore >= pointsToWin)
        {
            OnGameWon?.Invoke(1);
        }
        else
        {
            ResetRound();
        }
    }
    public void ComputerScore()
    {
        _computerScore++;
        OnScoreUpdated?.Invoke(2, _computerScore);
        

        if (_computerScore >= pointsToWin)
        {
            OnGameWon?.Invoke(2);
        }
        else
        {
            ResetRound();
        }
    }

    private void ResetRound()
    {
        this.playerPaddle.ResetPosition();
        this.computerPaddle.ResetPosition();
        this.ball.ResetPosition();
        this.ball.AddInitialForce();

        OnRoundReset?.Invoke();
    }


    // --Scene Navigation--
    public void PlayAgain()
    {
        Debug.Log("Play Again Clicked");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        PlayerPrefs.DeleteKey("Mode");
        SceneManager.LoadScene(0);
    }
}
