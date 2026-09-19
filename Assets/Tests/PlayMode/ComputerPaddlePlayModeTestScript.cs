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
        // 1. Set deterministic PlayerPrefs so Start() behaves predictably
        PlayerPrefs.SetInt("Mode", 0); // 1-Player (AI) mode
        PlayerPrefs.SetInt("Difficulty", 0); // Easy mode

        // Create Computer Paddle GameObject
        _paddleObject = new GameObject("ComputerPaddle");

        // Add Rigidbody2D FIRST and disable gravity so it doesn't fall during tests
        _paddleRigidbody = _paddleObject.AddComponent<Rigidbody2D>();
        _paddleRigidbody.gravityScale = 0f;

        // Add ComputerPaddle AFTER Rigidbody2D
        _computerPaddle = _paddleObject.AddComponent<ComputerPaddle>();

        // Create Ball GameObject
        _ballObject = new GameObject("Ball");
                                                                                                        
        // Add Rigidbody2D FIRST and disable gravity
        _ballRigidbody = _ballObject.AddComponent<Rigidbody2D>();
        _ballRigidbody.gravityScale = 0f;

        // Add Ball AFTER Rigidbody2D
        //_ballObject.AddComponent<Ball>();

        // Connect Ball to ComputerPaddle
        _computerPaddle.ball = _ballRigidbody;

        // Set the base speed so Easy mode initialization in Start() has a value to copy
        _computerPaddle.speed = 10f;

        // Override AI values for test consistency (these take over after Start)
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

    [UnityTest]
    public IEnumerator FixedUpdate_BallMovingAway_MovesPaddleTowardsCenter()
    {
        // Arrange
        // 1. Move the paddle far above the center (Y = 5). 
        // Even with a +0.6 random offset, the target is below it, forcing it DOWN.
        _paddleObject.transform.position = new Vector3(10f, 5f, 0f);
        _paddleRigidbody.gravityScale = 0f;

        // 2. Ensure the ball is moving LEFT (negative X), away from the right paddle
        _ballObject.transform.position = Vector3.zero;
        _ballRigidbody.velocity = new Vector2(-10f, 0f);

        // Act
        // 3. Yield enough times to safely bypass the AI's "frameCount % 8 == 0" frame skip
        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForFixedUpdate();
        }

        // Assert
        Assert.That(_paddleRigidbody.velocity.y, Is.LessThan(0f), "Computer paddle should move downward towards the center when the ball is moving away.");
    }
}