using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TestTools;

public class ScoringZonePlayModeTests
{
    private GameObject _scoringZoneObject;
    private ScoringZone _scoringZone;
    private GameObject _ballObject;
    private Ball _ball;
    private Rigidbody2D _ballRb;

    private bool _scoreTriggerFired;

    [SetUp]
    public void SetUp()
    {
        _scoreTriggerFired = false;

        // Create Scoring Zone
        _scoringZoneObject = new GameObject("ScoringZone");
        _scoringZoneObject.transform.position = new Vector3(10f, 0f, 0f);
        Rigidbody2D zoneRb = _scoringZoneObject.AddComponent<Rigidbody2D>();
        zoneRb.bodyType = RigidbodyType2D.Static;
        BoxCollider2D zoneCollider = _scoringZoneObject.AddComponent<BoxCollider2D>();
        zoneCollider.size = new Vector2(1f, 10f);

        _scoringZone = _scoringZoneObject.AddComponent<ScoringZone>();
        _scoringZone.scoreTrigger = new EventTrigger.TriggerEvent();
        _scoringZone.scoreTrigger.AddListener((data) => { _scoreTriggerFired = true; });

        // Create Ball
        _ballObject = new GameObject("Ball");
        _ballObject.transform.position = new Vector3(8.5f, 0f, 0f);
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
        if (_scoringZoneObject != null) Object.Destroy(_scoringZoneObject);
        if (_ballObject != null) Object.Destroy(_ballObject);
    }

    [UnityTest]
    public IEnumerator ScoringZone_WhenBallCollides_TriggersScoreEvent()
    {
        // Arrange: launch ball right into the scoring zone
        _ballRb.velocity = new Vector2(10f, 0f);

        // Act
        // At 10 units/sec, traveling 0.5 units takes 0.05s. Wait 0.1s to guarantee impact.
        yield return new WaitForSeconds(0.1f);

        // Assert
        Assert.IsTrue(_scoreTriggerFired, "ScoringZone scoreTrigger should be invoked when a Ball enters the zone.");
    }

    [UnityTest]
    public IEnumerator ScoringZone_WhenNonBallCollides_DoesNotTriggerScore()
    {
        // Arrange: remove Ball component from ball object so it's a non-ball collider
        Object.DestroyImmediate(_ball);
        _ballRb.velocity = new Vector2(10f, 0f);

        // Act
        yield return new WaitForSeconds(0.1f);

        // Assert
        Assert.IsFalse(_scoreTriggerFired, "ScoringZone scoreTrigger should NOT be invoked by a non-Ball object.");
    }
}
