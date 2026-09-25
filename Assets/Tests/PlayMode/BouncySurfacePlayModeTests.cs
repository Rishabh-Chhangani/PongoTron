using System.Collections;
using System.Threading;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BouncySurfacePlayModeTests
{
    private GameObject _surfaceObject;
    private BouncySurface _bouncySurface;
    private GameObject _ballObject;
    private Ball _ball;
    private Rigidbody2D _ballRb;

    [SetUp]
    public void SetUp()
    {
        // Create Bouncy Surface
        _surfaceObject = new GameObject("BouncySurface");
        _surfaceObject.transform.position = new Vector3(0f, 2f, 0f);
        Rigidbody2D surfaceRb = _surfaceObject.AddComponent<Rigidbody2D>();
        surfaceRb.bodyType = RigidbodyType2D.Static;
        BoxCollider2D surfaceCollider = _surfaceObject.AddComponent<BoxCollider2D>();
        surfaceCollider.size = new Vector2(10f, 1f);
        _bouncySurface = _surfaceObject.AddComponent<BouncySurface>();
        _bouncySurface.bounceStrnegth = 200f;

        // Create Ball
        _ballObject = new GameObject("Ball");
        _ballObject.transform.position = new Vector3(0f, 1.2f, 0f);
        _ballRb = _ballObject.AddComponent<Rigidbody2D>();
        _ballRb.gravityScale = 0f;
        CircleCollider2D ballCollider = _ballObject.AddComponent<CircleCollider2D>();
        ballCollider.radius = 0.5f;
        _ball = _ballObject.AddComponent<Ball>();
        _ball.InitializeComponents();
    }

    [TearDown]
    public void TearDown()
    {
        if (_surfaceObject != null) Object.Destroy(_surfaceObject);
        if (_ballObject != null) Object.Destroy(_ballObject);
    }

    [UnityTest]
    public IEnumerator BouncySurface_OnCollisionEnter2D_AppliesBounceForceToBall()
    {
        // Arrange: Move ball down to Y=0 to prevent instant overlap.
        // Surface bottom is at Y=1.5, Ball top is at Y=0.5. Gap = 1.0 units.
        _ballObject.transform.position = Vector3.zero;
        _ballRb.position = Vector2.zero; // Ensure Rigidbody position is perfectly synced

        // Launch ball upwards
        _ballRb.velocity = new Vector2(0f, 10f);

        // Act: At 10 units/sec, traveling the 1 unit gap takes 0.1 seconds. 
        // Wait 0.15s to guarantee impact and allow OnCollisionEnter2D to process.
        float timeout = Time.time + 1f;
        yield return new WaitUntil(() => _ballRb.velocity.y < 0f || Time.time > timeout);

        // Assert: contact normal points downward from surface, so -normal * strength pushes downward
        Assert.That(_ballRb.velocity.y, Is.LessThan(0f), "Ball should reflect and bounce downward after hitting top bouncy surface.");
    }
}
