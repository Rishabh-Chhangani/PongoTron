
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

}
