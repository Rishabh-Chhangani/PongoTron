using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerPaddlePlayModeTests
{
    private GameObject _paddleObject;
    private PlayerPaddle _playerPaddle;
    private Rigidbody2D _paddleRb;

    [SetUp]
    public void SetUp()
    {
        _paddleObject = new GameObject("PlayerPaddle");
        _paddleRb = _paddleObject.AddComponent<Rigidbody2D>();
        _paddleRb.gravityScale = 0f;
        _playerPaddle = _paddleObject.AddComponent<PlayerPaddle>();
        _playerPaddle.speed = 10f;
    }

    [TearDown]
    public void TearDown()
    {
        if (_paddleObject != null) Object.Destroy(_paddleObject);
    }

    [UnityTest]
    public IEnumerator PlayerPaddle_WithoutInput_MaintainsZeroVelocity()
    {
        // Act
        yield return new WaitForFixedUpdate();

        // Assert
        Assert.AreEqual(Vector2.zero, _paddleRb.velocity, "Paddle velocity should remain zero without input.");
    }

    [UnityTest]
    public IEnumerator PlayerPaddle_ResetPosition_ResetsPositionAndStopsVelocity()
    {
        // Arrange
        _paddleRb.position = new Vector2(-8f, 4f);
        _paddleRb.velocity = new Vector2(0f, 6f);

        // Act
        _playerPaddle.ResetPosition();
        yield return new WaitForFixedUpdate();

        // Assert
        Assert.AreEqual(-8f, _paddleRb.position.x, 0.01f, "Paddle X position must be preserved.");
        Assert.AreEqual(0f, _paddleRb.position.y, 0.01f, "Paddle Y position must be 0.");
        Assert.AreEqual(Vector2.zero, _paddleRb.velocity, "Paddle velocity must be zero.");
    }

    [UnityTest]
    public IEnumerator PlayerPaddle_DirectionSet_AppliesForceInFixedUpdate()
    {
        // Arrange
        // Disable the script so the Update() loop cannot reset our injected input
        _playerPaddle.enabled = false;

        FieldInfo dirField = typeof(PlayerPaddle).GetField("_direction", BindingFlags.NonPublic | BindingFlags.Instance);
        dirField.SetValue(_playerPaddle, Vector2.up);

        // Act
        // Manually invoke FixedUpdate to bypass the engine's disabled state
        MethodInfo fixedUpdateMethod = typeof(PlayerPaddle).GetMethod("FixedUpdate", BindingFlags.NonPublic | BindingFlags.Instance);
        fixedUpdateMethod.Invoke(_playerPaddle, null);

        // Yield to let the Rigidbody actually process the velocity/force change
        yield return new WaitForFixedUpdate();

        // Assert
        Assert.That(_paddleRb.velocity.y, Is.GreaterThan(0f), "Paddle should accelerate upwards when direction is set to up.");
    }
}
