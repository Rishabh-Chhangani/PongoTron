using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BallPlayModeTests
{
    private GameObject _ballObject;
    private Ball _ball;
    private Rigidbody2D _ballRb;
    private CircleCollider2D _ballCollider;

    private GameObject _otherObject;

    private Ball.HitType? _lastHitType;

    [SetUp]
    public void SetUp()
    {
        _lastHitType = null;
        Ball.OnBallCollided += HandleBallCollided;

        _ballObject = new GameObject("Ball");
        _ballRb = _ballObject.AddComponent<Rigidbody2D>();
        _ballRb.gravityScale = 0f;
        _ballCollider = _ballObject.AddComponent<CircleCollider2D>();
        _ballCollider.radius = 0.5f;


        // ADD THESE 4 LINES to create a bouncy material
        PhysicsMaterial2D bouncyMat = new PhysicsMaterial2D("Bouncy");
        bouncyMat.bounciness = 1f;
        bouncyMat.friction = 0f;
        _ballCollider.sharedMaterial = bouncyMat;

        _ball = _ballObject.AddComponent<Ball>();
        _ball.InitializeComponents();
    }

    [TearDown]
    public void TearDown()
    {
        Ball.OnBallCollided -= HandleBallCollided;

        if (_ballObject != null) Object.Destroy(_ballObject);
        if (_otherObject != null) Object.Destroy(_otherObject);
    }

    private void HandleBallCollided(Ball.HitType hitType)
    {
        _lastHitType = hitType;
    }

    [UnityTest]
    public IEnumerator Ball_AddInitialForce_ProducesMovementOverTime()
    {
        // Arrange
        Vector2 startPos = Vector2.zero;
        _ballRb.position = startPos;

        // Act
        _ball.AddInitialForce();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();

        // Assert
        Assert.That(_ballRb.position, Is.Not.EqualTo(startPos), "Ball should move after AddInitialForce is applied.");
    }

    [UnityTest]
    public IEnumerator Ball_CollisionWithPaddle_FiresPaddleHitEventAndIncreasesSpeed()
    {
        // Arrange
        _otherObject = new GameObject("Paddle");
        Rigidbody2D paddleRb = _otherObject.AddComponent<Rigidbody2D>();
        paddleRb.bodyType = RigidbodyType2D.Kinematic;
        BoxCollider2D paddleCol = _otherObject.AddComponent<BoxCollider2D>();
        paddleCol.size = new Vector2(1f, 4f);
        _otherObject.AddComponent<Paddle>();
        _otherObject.transform.position = new Vector3(1f, 0f, 0f);

        _ballObject.transform.position = Vector3.zero;
        float originalSpeed = _ball.speed;
        _ballRb.velocity = new Vector2(10f, 0f);

        // Act
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();

        // Assert
        Assert.AreEqual(Ball.HitType.Paddle, _lastHitType, "Collision with paddle should trigger Paddle HitType.");
        Assert.That(_ballRb.velocity.magnitude, Is.GreaterThan(originalSpeed), "Collision with paddle should increase ball speed.");
    }

    [UnityTest]
    public IEnumerator Ball_CollisionWithWall_FiresWallHitEvent()
    {
        // Arrange
        _otherObject = new GameObject("Wall");
        Rigidbody2D wallRb = _otherObject.AddComponent<Rigidbody2D>();
        wallRb.bodyType = RigidbodyType2D.Kinematic;
        BoxCollider2D wallCol = _otherObject.AddComponent<BoxCollider2D>();
        wallCol.size = new Vector2(10f, 1f);
        _otherObject.AddComponent<Wall>();
        _otherObject.transform.position = new Vector3(0f, 1f, 0f);

        _ballObject.transform.position = Vector3.zero;
        _ballRb.velocity = new Vector2(0f, 10f);

        // Act
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();

        // Assert
        Assert.AreEqual(Ball.HitType.Wall, _lastHitType, "Collision with wall should trigger Wall HitType.");
    }
}
