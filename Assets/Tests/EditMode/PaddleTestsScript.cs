using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PaddleTestsScript
{

    
   
    private GameObject _dummyPlayerPaddle;
    private Paddle _paddle;

    private Rigidbody2D _playerRigidbody;

    [SetUp]
    public void SetUp()
    {
        _dummyPlayerPaddle = new GameObject("Paddle");
        _paddle = _dummyPlayerPaddle.AddComponent<Paddle>();
        _playerRigidbody = _dummyPlayerPaddle.AddComponent<Rigidbody2D>();
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(_dummyPlayerPaddle);
    }

    [Test]
    public void ResetPosition_ResetsPositionAndVelocity()
    {
        //Arrange
        _playerRigidbody.position = new Vector2(5, 5);
        _playerRigidbody.velocity = Vector2.up * 5;

        //Act
        _paddle.ResetPosition();

        //Assert
        Assert.AreEqual(5.0f, _playerRigidbody.position.x, 0.01f, "Paddle X position should remain unchanged.");
        Assert.AreEqual(0.0f, _playerRigidbody.position.y, 0.01f, "Paddle Y position should be reset to 0.");
        Assert.AreEqual(0.0f, _playerRigidbody.velocity.y, 0.01f, "Paddle Y velocity should be reset to 0.");
    }

    [Test]
    public void Paddle_DefaultSpeed_IsConfiguredToTen()
    {
        Assert.AreEqual(10.0f, _paddle.speed, 0.001f, "Paddle default speed should be 10.0f.");
    }

    [Test]
    public void ResetPosition_WhenRigidbodyNotPreAssigned_InitializesComponent()
    {
        // Arrange
        GameObject newPaddleObj = new GameObject("LazyPaddle");
        newPaddleObj.AddComponent<Rigidbody2D>();
        Paddle newPaddle = newPaddleObj.AddComponent<Paddle>();

        // Act
        newPaddle.ResetPosition();

        // Assert
        Rigidbody2D rb = newPaddleObj.GetComponent<Rigidbody2D>();
        Assert.AreEqual(0.0f, rb.position.y, 0.001f, "Paddle Y position should reset to 0 even if Awake wasn't called.");
        Assert.AreEqual(Vector2.zero, rb.velocity, "Paddle velocity should reset to zero.");

        Object.DestroyImmediate(newPaddleObj);
    }

    [Test]
    public void PlayerPaddle_InheritsPaddleBehavior_ResetsPositionAndZeroesVelocity()
    {
        // Arrange
        GameObject playerPaddleObj = new GameObject("PlayerPaddle");
        Rigidbody2D rb = playerPaddleObj.AddComponent<Rigidbody2D>();
        PlayerPaddle playerPaddle = playerPaddleObj.AddComponent<PlayerPaddle>();

        rb.position = new Vector2(-8f, 3.5f);
        rb.velocity = new Vector2(0f, -4f);

        // Act
        playerPaddle.ResetPosition();

        // Assert
        Assert.AreEqual(-8.0f, rb.position.x, 0.01f, "Player paddle X position should be preserved.");
        Assert.AreEqual(0.0f, rb.position.y, 0.01f, "Player paddle Y position should reset to 0.");
        Assert.AreEqual(Vector2.zero, rb.velocity, "Player paddle velocity should reset to zero.");

        Object.DestroyImmediate(playerPaddleObj);
    }
}

