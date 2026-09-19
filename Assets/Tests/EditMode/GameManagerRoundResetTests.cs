using NUnit.Framework;
using UnityEngine;

public class GameManagerRoundResetTests
{
    private GameObject _gmObject;
    private GameManager _gm;
    private GameObject _playerPaddleObject;
    private GameObject _computerPaddleObject;
    private GameObject _ballObject;

    private bool _roundResetFired;

    [SetUp]
    public void SetUp()
    {
        _gmObject = new GameObject("TestGameManager");
        _gm = _gmObject.AddComponent<GameManager>();

        _playerPaddleObject = new GameObject("PlayerPaddle");
        _playerPaddleObject.AddComponent<Rigidbody2D>();
        _gm.playerPaddle = _playerPaddleObject.AddComponent<Paddle>();

        _computerPaddleObject = new GameObject("ComputerPaddle");
        _computerPaddleObject.AddComponent<Rigidbody2D>();
        _gm.computerPaddle = _computerPaddleObject.AddComponent<Paddle>();

        _ballObject = new GameObject("Ball");
        _ballObject.AddComponent<Rigidbody2D>();
        _gm.ball = _ballObject.AddComponent<Ball>();

        _roundResetFired = false;
        GameManager.OnRoundReset += HandleRoundReset;
    }

    [TearDown]
    public void TearDown()
    {
        GameManager.OnRoundReset -= HandleRoundReset;

        if (_gmObject != null) Object.DestroyImmediate(_gmObject);
        if (_playerPaddleObject != null) Object.DestroyImmediate(_playerPaddleObject);
        if (_computerPaddleObject != null) Object.DestroyImmediate(_computerPaddleObject);
        if (_ballObject != null) Object.DestroyImmediate(_ballObject);
    }

    private void HandleRoundReset()
    {
        _roundResetFired = true;
    }

    [Test]
    public void PlayerScore_BelowWinningPoints_FiresRoundResetEventAndResetsEntities()
    {
        // Arrange
        _gm.pointsToWin = 3;
        _ballObject.GetComponent<Rigidbody2D>().position = new Vector2(5f, 2f);
        _playerPaddleObject.GetComponent<Rigidbody2D>().position = new Vector2(-8f, 3f);
        _computerPaddleObject.GetComponent<Rigidbody2D>().position = new Vector2(8f, -2f);

        // Act
        _gm.PlayerScore();

        // Assert
        Assert.IsTrue(_roundResetFired, "OnRoundReset event should fire when score is below pointsToWin.");
        Assert.AreEqual(0f, _ballObject.GetComponent<Rigidbody2D>().position.x, 0.001f, "Ball X position must be reset to 0.");
        Assert.AreEqual(0f, _playerPaddleObject.GetComponent<Rigidbody2D>().position.y, 0.001f, "Player paddle Y position must reset to 0.");
        Assert.AreEqual(0f, _computerPaddleObject.GetComponent<Rigidbody2D>().position.y, 0.001f, "Computer paddle Y position must reset to 0.");
    }

    [Test]
    public void ComputerScore_BelowWinningPoints_FiresRoundResetEventAndResetsEntities()
    {
        // Arrange
        _gm.pointsToWin = 3;
        _ballObject.GetComponent<Rigidbody2D>().position = new Vector2(-5f, -2f);

        // Act
        _gm.ComputerScore();

        // Assert
        Assert.IsTrue(_roundResetFired, "OnRoundReset event should fire when computer scores below pointsToWin.");
        Assert.AreEqual(0f, _ballObject.GetComponent<Rigidbody2D>().position.x, 0.001f, "Ball X position must be reset to 0.");
    }

    [Test]
    public void PlayerScore_ReachingPointsToWin_DoesNotFireRoundReset()
    {
        // Arrange
        _gm.pointsToWin = 1;

        // Act
        _gm.PlayerScore();

        // Assert
        Assert.IsFalse(_roundResetFired, "OnRoundReset event should NOT fire when a player wins.");
    }

    [Test]
    public void ComputerScore_ReachingPointsToWin_DoesNotFireRoundReset()
    {
        // Arrange
        _gm.pointsToWin = 1;

        // Act
        _gm.ComputerScore();

        // Assert
        Assert.IsFalse(_roundResetFired, "OnRoundReset event should NOT fire when computer wins.");
    }
}
