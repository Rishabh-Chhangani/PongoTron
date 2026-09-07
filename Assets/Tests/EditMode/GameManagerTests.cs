using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GameManagerTest
{
    // Setup Variables
    private GameObject _gmObject;
    private GameManager _gm;

    private GameObject _dummyPlayerPaddle;
    private GameObject _dummyComputerPaddle;
    private GameObject _dummyBall;

    // Tracking Variables
    private bool _eventFired;
    private int _recordedPlayerIndex;
    private int _recordedScore;
    private bool _isGameOverEventFired;
    private int _recordedWinnerIndex;
    private bool _isResetRoundFired;

    [SetUp]
    public void SetUp()
    {
        _gmObject = new GameObject("TestGameManger");
        _gm = _gmObject.AddComponent<GameManager>();

        // Create dummy game objects for the paddles and ball
        _dummyPlayerPaddle = new GameObject("PlayerPaddle");
        _dummyComputerPaddle = new GameObject("ComputerPaddle");
        _dummyBall = new GameObject("Ball");

        // Add Rigidbody2D to avoid NRE
        _dummyPlayerPaddle.AddComponent<Rigidbody2D>();
        _dummyComputerPaddle.AddComponent<Rigidbody2D>();
        _dummyBall.AddComponent<Rigidbody2D>();

        // Add and store the Paddle/Ball components
        _gm.playerPaddle = _dummyPlayerPaddle.AddComponent<Paddle>();
        _gm.computerPaddle = _dummyComputerPaddle.AddComponent<Paddle>();
        _gm.ball = _dummyBall.AddComponent<Ball>();

        // Reset tracking variables for a clean slate
        _eventFired = false;
        _isGameOverEventFired = false;
        _isResetRoundFired = false;
        _recordedPlayerIndex = -1;
        _recordedScore = -1;
        _recordedWinnerIndex = -1;
    }

    [TearDown]
    public void Teardown()
    {
        // 1. Unsubscribe from all events
        GameManager.OnRoundReset -= MockResetRoundListener;
        GameManager.OnGameWon -= MockGameOverListener;
        GameManager.OnScoreUpdated -= MockScoreListener;

        // 2. Reset Global Unity States
        Time.timeScale = 1f;
        PlayerPrefs.DeleteKey("Mode");

        // 3. Safely Destroy Objects
        if (_gmObject != null) Object.DestroyImmediate(_gmObject);
        if (_dummyPlayerPaddle != null) Object.DestroyImmediate(_dummyPlayerPaddle);
        if (_dummyComputerPaddle != null) Object.DestroyImmediate(_dummyComputerPaddle);
        if (_dummyBall != null) Object.DestroyImmediate(_dummyBall);
    }

    // --- MOCK LISTENERS ---

    private void MockResetRoundListener()
    {
        _isResetRoundFired = true;
    }

    private void MockScoreListener(int playerIndex, int newScore)
    {
        _eventFired = true;
        _recordedPlayerIndex = playerIndex;
        _recordedScore = newScore;
    }

    private void MockGameOverListener(int _winnerplayerIndex)
    {
        _isGameOverEventFired = true;
        _recordedWinnerIndex = _winnerplayerIndex;
    }

    // --- TESTS ---

    [Test]
    public void Resetround_WhenPlayerScores_TriggersResetRoundEvent()
    {
        // Arrange
        GameManager.OnRoundReset += MockResetRoundListener;

        // Act
        _gm.PlayerScore();

        // Assert
        Assert.IsTrue(_isResetRoundFired, "The reset round event did not fire!");
    }

    [Test]
    public void Resetround_WhenComputerScores_TriggersResetRoundEvent()
    {
        // Arrange
        GameManager.OnRoundReset += MockResetRoundListener;

        // Act
        _gm.ComputerScore();

        // Assert
        Assert.IsTrue(_isResetRoundFired, "The reset round event did not fire!");
    }

    [Test]
    public void PlayerScore_ReachedWinCondtion_TriggersGameOverEvent()
    {
        // Arrange
        GameManager.OnGameWon += MockGameOverListener;

        // Act
        for (int i = 0; i < _gm.pointsToWin; i++)
        {
            _gm.PlayerScore();
        }

        // Assert
        Assert.IsTrue(_isGameOverEventFired, "The game over event did not fire!");
        Assert.AreEqual(1, _recordedWinnerIndex, "The event broadcasted the wrong winner index!");
    }

    [Test]
    public void ComputerScore_ReachedWinCondition_TriggersGameOverEvent()
    {
        // Arrange
        GameManager.OnGameWon += MockGameOverListener;

        // Act
        for (int i = 0; i < _gm.pointsToWin; i++)
        {
            _gm.ComputerScore();
        }

        // Assert
        Assert.IsTrue(_isGameOverEventFired, "The Game Over event did not fire!");
        Assert.AreEqual(2, _recordedWinnerIndex, "The event broadcasted the wrong winner index!");
    }

    [Test]
    public void PlayerScore_IncrementsScore_AndFiresEvent()
    {
        // Arrange
        GameManager.OnScoreUpdated += MockScoreListener;

        // Act
        _gm.PlayerScore();

        // Assert
        Assert.IsTrue(_eventFired, "The score event did not fire!");
        Assert.AreEqual(1, _recordedPlayerIndex, "The event broadcasted the wrong player index!");
        Assert.AreEqual(1, _recordedScore, "The score did not increment to 1!");
    }

    [Test]
    public void ComputerScore_IncrementScore_AndFiresEvent()
    {
        // Arrange
        GameManager.OnScoreUpdated += MockScoreListener;

        // Act
        _gm.ComputerScore();

        // Assert
        Assert.IsTrue(_eventFired, "The Score event didn't fire!");
        Assert.AreEqual(2, _recordedPlayerIndex, "The event broadcasted the wrong player index!");
        Assert.AreEqual(1, _recordedScore, "The score did not increment to 1!");
    }

    [Test]
    public void PlayAgain_WhenCalled_ResetsTimeScaleToOne()
    {
        // Arrange
        Time.timeScale = 0;

        // Act 
        try
        {
            _gm.PlayAgain();
        }
        catch (System.InvalidOperationException)
        {
            // Intentional catch for SceneManager in EditMode
        }

        // Assert
        Assert.AreEqual(1f, Time.timeScale, "Time.timeScale was not reset to 1!");
    }

    [Test]
    public void MainMenu_WhenCalled_ResetsTimeScaleToOneAndPlayerPrefsToMode()
    {
        // Arrange
        Time.timeScale = 0;
        PlayerPrefs.SetInt("Mode", 2);

        // Act 
        try
        {
            _gm.MainMenu();
        }
        catch (System.InvalidOperationException)
        {
            // Intentional catch for SceneManager in EditMode
        }

        // Assert
        Assert.AreEqual(1f, Time.timeScale, "Time.timeScale was not reset to 1!");
        Assert.IsFalse(PlayerPrefs.HasKey("Mode"), "PlayerPrefs still contains the 'Mode' key!");
    }
}