using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Ball ball;
    public ScoreText playerScoreText;
    public ScoreText computerScoreText;

    [Header("Win Condition")]
    public GameObject gameOverPanel;

    public int pointsToWin = 5;
    public TMPro.TextMeshProUGUI winnerText;


    private int _playerScore;
    private int _computerScore;

    public Paddle playerPaddle;
    public Paddle computerPaddle;

    [SerializeField]private GameAudio gameAudio;

    [SerializeField] private ScoringZone playerScoringZone;
    [SerializeField] private ScoringZone computerScoringZone;

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
            playerScoreText.SetScore(_playerScore);
        }
        // Reset scores on fresh game start

        if (computerScoreText != null)
        {
            _computerScore = 0;
            computerScoreText.SetScore(_computerScore);
        }

        if (playerScoringZone != null)
        {
            playerScoringZone.OnBallScored += HandlePlayerScored;
        }

        if (computerScoringZone != null) // fixed null-check
        {
            computerScoringZone.OnBallScored += HandleComputerScored;
        }
    }


    private void Awake()
    {
        if(gameAudio == null)
        {
            gameAudio = GetComponent<GameAudio>();
        }
        if(gameObject == null)
        {
            Debug.LogError("GameManager GameObject is null!");
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
      
        playerScoreText.SetScore(_playerScore);
        computerScoreText.SetScore(_computerScore);
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

    private void HandlePlayerScored(ScoringZone zone)
    {
        PlayerScore();
    }

    private void HandleComputerScored(ScoringZone zone)
    {
        ComputerScore();
    }




    public void PlayerScore()
    {
        _playerScore++;
        playerScoreText.SetScore(_playerScore);
        playerScoreText.Highlight();
        Debug.Log($"Player Score:{_playerScore}, Win points {pointsToWin} ");

        if (_playerScore >= pointsToWin)
        {
            gameAudio.PlayWinSound();
            winnerText.text = "PLAYER 1 WINS!";
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);
        }
        else
        {
            gameAudio.PlayScoreSound();
            ResetRound();
        }
    }

    public void ComputerScore()
    {
        _computerScore++;
        computerScoreText.SetScore(_computerScore);
        computerScoreText.Highlight();

        if (_computerScore >= pointsToWin)
        {
            gameAudio.PlayWinSound();
            winnerText.text = "PLAYER 2 WINS!";
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);
        }
        else
        {
            Debug.Log("GameAudio reference: " + gameAudio);
            gameAudio.PlayScoreSound();
            ResetRound();
        }
    }


    private void OnDestroy()
    {
        if (playerScoringZone != null)
        {
            playerScoringZone.OnBallScored -= HandlePlayerScored;
        }

        if (computerScoringZone != null)
        {
            computerScoringZone.OnBallScored -= HandleComputerScored;
        }
    }
}
