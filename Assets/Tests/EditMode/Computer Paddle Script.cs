using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
public class ComputerPaddleTestScript
{
    private Ball _ball;
    private ComputerPaddle _computerPaddle;
    private Rigidbody2D _Rigidbody;

    [SetUp]
    public void SetUp()
    {
        // Create a dummy GameObject for the ba0ll
        GameObject ballObject = new GameObject("Ball");
        _ball = ballObject.AddComponent<Ball>();
        _Rigidbody = ballObject.AddComponent<Rigidbody2D>();
        // Create a dummy GameObject for the computer paddle
        GameObject computerPaddleObject = new GameObject("ComputerPaddle");
        computerPaddleObject.gameObject.AddComponent<Rigidbody2D>();
        _computerPaddle = computerPaddleObject.AddComponent<ComputerPaddle>();
        _computerPaddle.ball = _Rigidbody;

        _computerPaddle.Initilalize();
    }
    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(_ball.gameObject);
        Object.DestroyImmediate(_computerPaddle.gameObject);
    }
    [Test]
    public void ComputerPaddleTestScriptSimplePasses()
    { //Arrange
        _Rigidbody.position = new Vector2(5, 2);
        _Rigidbody.velocity = new Vector2(1, 5); // Ball moving towards the computer paddle
        _computerPaddle.transform.position = new Vector2(10, 0); // Computer paddle position
        _computerPaddle.currentPrediction = 1f;
        //Act
        float predictedY = _computerPaddle.GetPredictedY();
        //Assert
        Assert.AreEqual(27f, predictedY, 0.1f, "Predicted Y position should be approximately 27.");
    }

    [Test]
    public void GetPredictedY_WhenPredictionIsZeroOrNegative_ReturnsCurrentBallY()
    {
        // Arrange
        _Rigidbody.position = new Vector2(0f, 3.5f);
        _Rigidbody.velocity = new Vector2(5f, 2f);
        _computerPaddle.transform.position = new Vector3(10f, 0f, 0f);
        _computerPaddle.currentPrediction = 0.0f;

        // Act
        float predictedY = _computerPaddle.GetPredictedY();

        // Assert
        Assert.AreEqual(3.5f, predictedY, 0.001f, "When currentPrediction <= 0, should return current ball Y.");
    }

    [Test]
    public void GetPredictedY_WhenBallMovingAwayOrStationary_ReturnsCurrentBallY()
    {
        // Arrange: ball moving left (negative X) away from computer paddle at X=10
        _Rigidbody.position = new Vector2(0f, 2.0f);
        _Rigidbody.velocity = new Vector2(-5f, 2f);
        _computerPaddle.transform.position = new Vector3(10f, 0f, 0f);
        _computerPaddle.currentPrediction = 1.0f;

        // Act
        float predictedY = _computerPaddle.GetPredictedY();

        // Assert
        Assert.AreEqual(2.0f, predictedY, 0.001f, "When ball.velocity.x <= 0, should return current ball Y.");
    }

    [Test]
    public void GetPredictedY_WithFullPrediction_CalculatesExactInterceptY()
    {
        // Arrange:
        // Paddle at X=10, Ball at (0, 1). Velocity: X=5, Y=2.
        // distanceX = 10, time = 10 / 5 = 2s.
        // predictedY = 1 + (2 * 2 * 1.0) = 5.0f.
        _computerPaddle.transform.position = new Vector3(10f, 0f, 0f);
        _Rigidbody.position = new Vector2(0f, 1f);
        _Rigidbody.velocity = new Vector2(5f, 2f);
        _computerPaddle.currentPrediction = 1.0f;

        // Act
        float predictedY = _computerPaddle.GetPredictedY();

        // Assert
        Assert.AreEqual(5.0f, predictedY, 0.001f, "Predicted Y must accurately calculate linear trajectory intercept.");
    }

    [Test]
    public void GetPredictedY_WithPartialPrediction_CalculatesScaledInterceptY()
    {
        // Arrange:
        // Paddle at X=10, Ball at (0, 1). Velocity: X=5, Y=2.
        // distanceX = 10, time = 2s.
        // currentPrediction = 0.5f -> predictedY = 1 + (2 * 2 * 0.5) = 3.0f.
        _computerPaddle.transform.position = new Vector3(10f, 0f, 0f);
        _Rigidbody.position = new Vector2(0f, 1f);
        _Rigidbody.velocity = new Vector2(5f, 2f);
        _computerPaddle.currentPrediction = 0.5f;

        // Act
        float predictedY = _computerPaddle.GetPredictedY();

        // Assert
        Assert.AreEqual(3.0f, predictedY, 0.001f, "Predicted Y must scale proportionally with prediction factor.");
    }

    [Test]
    public void GetPredictedY_WithNegativeBallVelocityY_CalculatesCorrectIntercept()
    {
        // Arrange:
        // Paddle at X=10, Ball at (0, 1). Velocity: X=5, Y=-3.
        // distanceX = 10, time = 2s.
        // predictedY = 1 + (-3 * 2 * 1.0) = -5.0f.
        _computerPaddle.transform.position = new Vector3(10f, 0f, 0f);
        _Rigidbody.position = new Vector2(0f, 1f);
        _Rigidbody.velocity = new Vector2(5f, -3f);
        _computerPaddle.currentPrediction = 1.0f;

        // Act
        float predictedY = _computerPaddle.GetPredictedY();

        // Assert
        Assert.AreEqual(-5.0f, predictedY, 0.001f, "Predicted Y must accurately calculate downward intercept.");
    }

    [Test]
    public void CalculateMovementDirection_WhenDiffWithinDeadZone_SlowsPaddle()
    {
        // Arrange
        Rigidbody2D paddleRb = _computerPaddle.gameObject.GetComponent<Rigidbody2D>();
        paddleRb.position = new Vector2(10f, 2f);
        paddleRb.velocity = new Vector2(0f, 5f);
        _computerPaddle.currentDeadZone = 0.5f;
        _computerPaddle.currentSpeed = 10f;

        // Target Y within deadzone diff: |2.2 - 2.0| = 0.2 <= 0.5
        float targetY = 2.2f;

        // Act
        _computerPaddle.CalculateMovementDirection(targetY);

        // Assert: velocity should be lerped towards Vector2.zero (magnitude decreases)
        Assert.That(paddleRb.velocity.y, Is.LessThan(5f), "Paddle velocity should slow down towards zero inside dead zone.");
    }

    [Test]
    public void CalculateMovementDirection_WhenDiffPositive_MovesPaddleUp()
    {
        // Arrange
        Rigidbody2D paddleRb = _computerPaddle.gameObject.GetComponent<Rigidbody2D>();
        paddleRb.position = new Vector2(10f, 0f);
        paddleRb.velocity = Vector2.zero;
        _computerPaddle.currentDeadZone = 0.1f;
        _computerPaddle.currentSpeed = 10f;

        float targetY = 3.0f; // Above paddle

        // Act
        _computerPaddle.CalculateMovementDirection(targetY);

        // Assert
        Assert.That(paddleRb.velocity.y, Is.GreaterThan(0f), "Paddle should accelerate upwards when target is above deadzone.");
    }

    [Test]
    public void CalculateMovementDirection_WhenDiffNegative_MovesPaddleDown()
    {
        // Arrange
        Rigidbody2D paddleRb = _computerPaddle.gameObject.GetComponent<Rigidbody2D>();
        paddleRb.position = new Vector2(10f, 0f);
        paddleRb.velocity = Vector2.zero;
        _computerPaddle.currentDeadZone = 0.1f;
        _computerPaddle.currentSpeed = 10f;

        float targetY = -3.0f; // Below paddle

        // Act
        _computerPaddle.CalculateMovementDirection(targetY);

        // Assert
        Assert.That(paddleRb.velocity.y, Is.LessThan(0f), "Paddle should accelerate downwards when target is below deadzone.");
    }

    [Test]
    public void ResetPosition_InheritedFromPaddle_ResetsYToZeroAndPreservesX()
    {
        // Arrange
        Rigidbody2D paddleRb = _computerPaddle.gameObject.GetComponent<Rigidbody2D>();
        paddleRb.position = new Vector2(9f, 4f);
        paddleRb.velocity = new Vector2(0f, 8f);

        // Act
        _computerPaddle.ResetPosition();

        // Assert
        Assert.AreEqual(9f, paddleRb.position.x, 0.001f, "Paddle X position must be preserved.");
        Assert.AreEqual(0f, paddleRb.position.y, 0.001f, "Paddle Y position must reset to 0.");
        Assert.AreEqual(Vector2.zero, paddleRb.velocity, "Paddle velocity must reset to zero.");
    }

    [Test]
    public void CalculateTargetY_WhenBallMovingAway_TargetsCenterZeroWithOffset()
    {
        // Arrange: ball moving left (-5 in X) away from paddle
        _Rigidbody.position = new Vector2(0f, 5f);
        _Rigidbody.velocity = new Vector2(-5f, 0f);

        // Act
        float targetY = _computerPaddle.CalculateTargetY();

        // Assert: when ball.velocity.x <= 0, targetY is 0f + Random.Range(-0.2f, 0.2f) * (3 - difficulty)
        // For difficulty 0, max offset is +/- 0.6f
        Assert.That(targetY, Is.InRange(-0.7f, 0.7f), "When ball moves away, AI should target center court near Y=0.");
    }
}
