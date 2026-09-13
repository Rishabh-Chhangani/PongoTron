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



 

    
    
}
