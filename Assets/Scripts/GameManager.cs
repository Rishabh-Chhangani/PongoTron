using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update dnafek
    public Ball ball;
    public Text playerScoreText;
    public Text computerScoreText;

    [Header("Win Condition")]
    public GameObject gameOverPanel;
    // public UnityEngine.UI.Text gameOverText;
    public int pointsToWin = 5;
    public TMPro.TextMeshProUGUI winnerText;


    private int _playerScore;
    private int _computerScore;

    public Paddle playerPaddle;
    public Paddle computerPaddle;

    void Start()
    {
        // SINGLETON PATTERN - Destroy duplicates
        int numGameManagers = FindObjectsOfType<GameManager>().Length;
        if (numGameManagers > 1)
        {
            Destroy(gameObject);
            return;
        }

        if (playerScoreText != null)
        {
            _playerScore = 0;
            playerScoreText.text = "0";

        }
        // Reset scores on fresh game start

        if (computerScoreText != null)
        {
            _computerScore = 0;
            computerScoreText.text = "0";
        }
    }

    public void PlayerScore()
    {
        _playerScore++;
        playerScoreText.text = _playerScore.ToString();
        Debug.Log($"Player Score:{_playerScore}, Win points {pointsToWin} ");

        if (_playerScore >= pointsToWin)
        {
            winnerText.text = "PLAYER 1 WINS!";
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);
        }
        else
        {
            ResetRound();
        }
    }
    public void ComputerScore()
    {
        _computerScore++;
        computerScoreText.text = _computerScore.ToString();

        if (_computerScore >= pointsToWin)
        {
            winnerText.text = "PLAYER 2 WINS!";
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);
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
    }

    public void PlayAgain()
    {
        Debug.Log("Play Again Clicked");
        _playerScore = 0;
        _computerScore = 0;
        playerScoreText.text = "0";
        computerScoreText.text = "0";
        gameOverPanel.SetActive(false);
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
