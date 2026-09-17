using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
public class ComputerPaddleTestScript
{
    private Ball _ball;
    private ComputerPaddle _computerPaddle;
    private Rigidbody2D _ballRigidbody;
    [SetUp]
    public void SetUp()
    {
        // Create a dummy GameObject for the ba0ll
        GameObject ballObject = new GameObject("Ball");
        _ball = ballObject.AddComponent<Ball>();
        _ballRigidbody = ballObject.AddComponent<Rigidbody2D>();
        // Create a dummy GameObject for the computer paddle
        GameObject computerPaddleObject = new GameObject("ComputerPaddle");
        _computerPaddle = computerPaddleObject.AddComponent<ComputerPaddle>();
        _computerPaddle.ball = _ballRigidbody;
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
        _ballRigidbody.position = new Vector2(5, 2);
        _ballRigidbody.velocity = new Vector2(1, 5); // Ball moving towards the computer paddle
        _computerPaddle.transform.position = new Vector2(10, 0); // Computer paddle position
        _computerPaddle.currentPrediction = 1f;
        //Act
        float predictedY = _computerPaddle.GetPredictedY();
        //Assert
        Assert.AreEqual(27f, predictedY, 0.1f, "Predicted Y position should be approximately 27.");
    }
}