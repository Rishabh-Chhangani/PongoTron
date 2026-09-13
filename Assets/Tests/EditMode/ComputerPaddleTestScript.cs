using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ComputerPaddleTestScript
{


    private Ball _ball;

    private ComputerPaddle _computerPaddle;


    [SetUp]
    public void SetUp()
    {
        // Create a dummy GameObject for the ball
        GameObject ballObject = new GameObject("Ball");
        _ball = ballObject.AddComponent<Ball>();
        _ball.InitializeComponents();
        // Create a dummy GameObject for the computer paddle
        GameObject computerPaddleObject = new GameObject("ComputerPaddle");
        _computerPaddle = computerPaddleObject.AddComponent<ComputerPaddle>();
        _computerPaddle.ball = _ball.GetComponent<Rigidbody2D>();
        // Set PlayerPrefs for testing
        PlayerPrefs.SetInt("Mode", 0); // Single Player Mode
        PlayerPrefs.SetInt("Difficulty", 1); // Medium Difficulty
    }

    [Test]
    public void ComputerPaddleTestScriptSimplePasses()
    {
        
    }

}
