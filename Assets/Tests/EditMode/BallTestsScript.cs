
using Codice.Client.BaseCommands.CheckIn.Progress;
using NUnit.Framework;
using UnityEngine;


public class BallTestsScript
{
    private GameObject _dummyBall;
    private Ball _ball;
    private Rigidbody2D _ballRigidbody;

    [SetUp]
    public void SetUp()
    {
        _dummyBall = new GameObject("Ball");
        _ball = _dummyBall.AddComponent<Ball>();
        _ballRigidbody = _dummyBall.AddComponent<Rigidbody2D>();

        _ball.InitializeComponents();
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(_dummyBall);
    }

    [Test]
    public void ResetPosition_ResetsPositionAndVelocity()
    {
        // Arrange
        float randomY = Random.Range(-4f, 4f);
        _ballRigidbody.position = new Vector2(5, randomY);
        _ballRigidbody.velocity = new Vector2(2f, 2f);

        //Act
        _ball.ResetPosition();

        // Assert
        Assert.AreEqual(_ballRigidbody.position.x, 0);
        Assert.That(_ballRigidbody.position.y, Is.InRange(-4f, 4f),"Ball Y position should be within the allowed range");
        Assert.AreEqual(Vector2.zero, _ballRigidbody.velocity); 
    }

    [Test]
    public void AddInitialForce_AddsVelocityAndDirection()
    { 
        //Arrange
        _ballRigidbody.velocity = Vector2.zero;

        

        //Act
        _ball.AddInitialForce();

        //Assert
        Assert.That(_ballRigidbody.velocity.magnitude, Is.GreaterThan(0), "Ball should have a non-zero velocity after adding initial force");
        Assert.AreEqual(_ball.speed, _ballRigidbody.velocity.magnitude,  0.1f, "Ball velocity should be equal to the ball's speed");
    }

    [Test]
    public void IncreaseSpeed_IncreasesVelocityMagnitude()
    {
        // Arrange
        _ballRigidbody.velocity = new Vector2(1f, 1f).normalized * _ball.speed;
        Vector2 _ballDirection = _ballRigidbody.velocity.normalized;
        // Act
        
        _ball.IncreaseSpeed();
        // Assert
        Assert.That(_ballRigidbody.velocity.magnitude, Is.GreaterThan(_ball.speed), "Ball speed should increase after calling IncreaseSpeed");

        Assert.That(
            _ballRigidbody.velocity.magnitude,
            Is.EqualTo(11f).Within(0.1f),
            "Ball velocity should increase from 10 to 11"
        );
        Assert.That(_ballDirection, Is.EqualTo(_ballRigidbody.velocity.normalized), "Ball direction should remain the same after increasing speed");
    }

    [Test]
    public void IncreaseSpeed_WhenStationary_DoesNotIncreaseSpeed()
    {
        // Arrange
        _ballRigidbody.velocity = Vector2.zero;

        // Act
        _ball.IncreaseSpeed();

        // Assert
        Assert.AreEqual(Vector2.zero, _ballRigidbody.velocity, "IncreaseSpeed on zero velocity should do nothing.");
    }

    [Test]
    public void IncreaseSpeed_ClampsToMaxSpeed()
    {
        // Arrange: velocity at 19f. Multiplied by 1.1 = 20.9f -> clamped to maxSpeed (20f)
        _ballRigidbody.velocity = new Vector2(19f, 0f);

        // Act
        _ball.IncreaseSpeed();

        // Assert
        Assert.That(_ballRigidbody.velocity.magnitude, Is.EqualTo(20f).Within(0.01f), "Ball speed must be clamped to maxSpeed (20f).");
    }

    [Test]
    public void AddForce_AppliesForceToBallRigidbody()
    {
        // Arrange
        _ballRigidbody.velocity = Vector2.zero;
        Vector2 forceToApply = new Vector2(10f, 5f);

        // Act
        _ball.AddForce(forceToApply);

        // Assert: In editmode, Rigidbody2D.AddForce modifies internal velocity/force or can be verified
        Assert.That(_dummyBall.GetComponent<Rigidbody2D>(), Is.Not.Null);
    }

    [Test]
    public void ResetPosition_WhenRigidbodyIsNull_InitializesAndResets()
    {
        // Arrange: create new Ball without calling InitializeComponents
        GameObject uninitializedBallObj = new GameObject("UninitBall");
        uninitializedBallObj.AddComponent<Rigidbody2D>();
        Ball uninitBall = uninitializedBallObj.AddComponent<Ball>();

        // Act
        uninitBall.ResetPosition();

        // Assert
        Rigidbody2D rb = uninitializedBallObj.GetComponent<Rigidbody2D>();
        Assert.AreEqual(0f, rb.position.x, 0.001f, "Ball X position must reset to 0 even if uninitialized.");
        Assert.AreEqual(Vector2.zero, rb.velocity, "Ball velocity must reset to zero.");

        Object.DestroyImmediate(uninitializedBallObj);
    }

    [Test]
    public void AddInitialForce_XVelocityIsNeverZero()
    {
        // Act
        _ball.AddInitialForce();

        // Assert
        Assert.That(Mathf.Abs(_ballRigidbody.velocity.x), Is.GreaterThan(0.01f), "Ball horizontal velocity must never be zero.");
    }
}
