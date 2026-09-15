using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ComputerPaddlePlayModeTest
{
    private GameObject _paddleObject;
    private GameObject _ballObject;

    private ComputerPaddle _computerPaddle;
    private Rigidbody2D _paddleRigidbody;
    private Rigidbody2D _ballRigidbody;

    [SetUp]
    public void SetUp()
    {
        // Create Computer Paddle GameObject
        _paddleObject = new GameObject("ComputerPaddle");

        // Add Rigidbody2D FIRST
        _paddleRigidbody = _paddleObject.AddComponent<Rigidbody2D>();

        // Add ComputerPaddle AFTER Rigidbody2D
        _computerPaddle = _paddleObject.AddComponent<ComputerPaddle>();


        // Create Ball GameObject
        _ballObject = new GameObject("Ball");

        // Add Rigidbody2D FIRST
        _ballRigidbody = _ballObject.AddComponent<Rigidbody2D>();

        // Add Ball AFTER Rigidbody2D
        _ballObject.AddComponent<Ball>();


        // Connect Ball to ComputerPaddle
        _computerPaddle.ball = _ballRigidbody;

        // Configure ComputerPaddle
        _computerPaddle.currentSpeed = 10f;
        _computerPaddle.currentDeadZone = 0f;
        _computerPaddle.currentPrediction = 0f;

        // Set positions
        _paddleRigidbody.position = new Vector2(10f, 0f);
        _ballRigidbody.position = new Vector2(0f, 5f);

        // Ball moving toward the ComputerPaddle
        _ballRigidbody.velocity = new Vector2(5f, 0f);
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(_paddleObject);
        Object.Destroy(_ballObject);
    }

    [UnityTest]
    public IEnumerator FixedUpdate_MovesPaddleTowardsBall()
    {
        // Act
        yield return new WaitForFixedUpdate();

        // Assert
        Assert.That(
            _paddleRigidbody.velocity.y,
            Is.GreaterThan(0f),
            "Computer paddle should move upward towards the ball."
        );
    }
}