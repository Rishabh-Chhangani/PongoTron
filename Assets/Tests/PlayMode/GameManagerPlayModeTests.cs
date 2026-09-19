using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GameManagerPlayModeTests
{
    private GameObject _gmObject;
    private GameManager _gm;

    private GameObject _playerPaddleObject;
    private GameObject _computerPaddleObject;
    private GameObject _ballObject;

    private Rigidbody2D _ballRb;
    private Rigidbody2D _playerPaddleRb;
    private Rigidbody2D _computerPaddleRb;

    private bool _scoreUpdatedFired;
    private int _lastScorePlayerIndex;
    private int _lastScoreValue;

    private bool _gameWonFired;
    private int _lastWinningPlayerIndex;

    private bool _roundResetFired;

    [SetUp]
    public void SetUp()
    {
        _scoreUpdatedFired = false;
        _gameWonFired = false;
        _roundResetFired = false;
        _lastScorePlayerIndex = -1;
        _lastScoreValue = -1;
        _lastWinningPlayerIndex = -1;

        GameManager.OnScoreUpdated += HandleScoreUpdated;
        GameManager.OnGameWon += HandleGameWon;
        GameManager.OnRoundReset += HandleRoundReset;

        _gmObject = new GameObject("GameManager");
        _gm = _gmObject.AddComponent<GameManager>();

        _playerPaddleObject = new GameObject("PlayerPaddle");
        _playerPaddleRb = _playerPaddleObject.AddComponent<Rigidbody2D>();
        _playerPaddleRb.gravityScale = 0f;
        _gm.playerPaddle = _playerPaddleObject.AddComponent<Paddle>();

        _computerPaddleObject = new GameObject("ComputerPaddle");
        _computerPaddleRb = _computerPaddleObject.AddComponent<Rigidbody2D>();
        _computerPaddleRb.gravityScale = 0f;
        _gm.computerPaddle = _computerPaddleObject.AddComponent<Paddle>();

        _ballObject = new GameObject("Ball");
        _ballRb = _ballObject.AddComponent<Rigidbody2D>();
        _ballRb.gravityScale = 0f;
        _gm.ball = _ballObject.AddComponent<Ball>();
        _gm.ball.InitializeComponents();
    }

    [TearDown]
    public void TearDown()
    {
        GameManager.OnScoreUpdated -= HandleScoreUpdated;
        GameManager.OnGameWon -= HandleGameWon;
        GameManager.OnRoundReset -= HandleRoundReset;

        Time.timeScale = 1f;

        if (_gmObject != null) Object.Destroy(_gmObject);
        if (_playerPaddleObject != null) Object.Destroy(_playerPaddleObject);
        if (_computerPaddleObject != null) Object.Destroy(_computerPaddleObject);
        if (_ballObject != null) Object.Destroy(_ballObject);
    }

    private void HandleScoreUpdated(int playerIndex, int score)
    {
        _scoreUpdatedFired = true;
        _lastScorePlayerIndex = playerIndex;
        _lastScoreValue = score;
    }

    private void HandleGameWon(int winningPlayerIndex)
    {
        _gameWonFired = true;
        _lastWinningPlayerIndex = winningPlayerIndex;
    }

    private void HandleRoundReset()
    {
        _roundResetFired = true;
    }

    [UnityTest]
    public IEnumerator GameManager_PlayerScore_ResetsRoundAndLaunchesBall()
    {
        // Arrange
        _gm.pointsToWin = 3;
        _ballRb.position = new Vector2(5f, 2f);

        // Act
        _gm.PlayerScore();
        yield return new WaitForFixedUpdate();

        // Assert
        Assert.IsTrue(_scoreUpdatedFired, "OnScoreUpdated should fire.");
        Assert.AreEqual(1, _lastScorePlayerIndex, "Player index should be 1.");
        Assert.AreEqual(1, _lastScoreValue, "Score should be 1.");
        Assert.IsTrue(_roundResetFired, "OnRoundReset should fire when score < pointsToWin.");
        Assert.That(_ballRb.velocity.magnitude, Is.GreaterThan(0f), "Ball should have velocity after round reset.");
    }

    [UnityTest]
    public IEnumerator GameManager_ComputerScore_ResetsRoundAndFiresEvent()
    {
        // Arrange
        _gm.pointsToWin = 3;

        // Act
        _gm.ComputerScore();
        yield return new WaitForFixedUpdate();

        // Assert
        Assert.IsTrue(_scoreUpdatedFired, "OnScoreUpdated should fire.");
        Assert.AreEqual(2, _lastScorePlayerIndex, "Player index should be 2 for computer.");
        Assert.AreEqual(1, _lastScoreValue, "Score should be 1.");
        Assert.IsTrue(_roundResetFired, "OnRoundReset should fire.");
    }

    [UnityTest]
    public IEnumerator GameManager_PlayerReachesWinThreshold_TriggersGameWonAndHaltRoundReset()
    {
        // Arrange
        _gm.pointsToWin = 2;

        // Act: score first point
        _gm.PlayerScore();
        yield return new WaitForFixedUpdate();
        Assert.IsFalse(_gameWonFired, "Game should not be won yet.");

        _roundResetFired = false;

        // Act: score winning point
        _gm.PlayerScore();
        yield return new WaitForFixedUpdate();

        // Assert
        Assert.IsTrue(_gameWonFired, "OnGameWon should fire when score reaches pointsToWin.");
        Assert.AreEqual(1, _lastWinningPlayerIndex, "Player 1 should be the winner.");
        Assert.IsFalse(_roundResetFired, "OnRoundReset should NOT fire when game is won.");
    }

    [UnityTest]
    public IEnumerator GameManager_ComputerReachesWinThreshold_TriggersGameWonForComputer()
    {
        // Arrange
        _gm.pointsToWin = 1;

        // Act
        _gm.ComputerScore();
        yield return new WaitForFixedUpdate();

        // Assert
        Assert.IsTrue(_gameWonFired, "OnGameWon should fire when computer reaches win score.");
        Assert.AreEqual(2, _lastWinningPlayerIndex, "Computer (Player 2) should be broadcasted as winner.");
    }
}
