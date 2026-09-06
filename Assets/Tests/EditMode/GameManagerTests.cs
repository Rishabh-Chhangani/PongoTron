using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GameManagerTest
{



    // Variables to track the event firing and the recorded values
    private bool _eventFired;
    private int _recordedPlayerIndex;
    private int _recordedScore;

    // A mock listener to simulate a subscriber to the event
    private void MockScoreListener(int playerIndex, int newScore)
    {
        _eventFired = true;
        _recordedPlayerIndex = playerIndex;
        _recordedScore = newScore;
    }

    // A Test behaves as an ordinary method
    [Test]
    public void PlayerScore_IncrementsScore_AndFiresEvent()
    {
        // 1. ARRANGE: Set up the environment
        GameObject gmObject = new GameObject("TestGameManager");
        GameManager gm = gmObject.AddComponent<GameManager>();

        //Creating Dummy game objects for ball, PlayerPaddle, CoputerPaddle.To pass the NullPointerException.
        GameObject dummyPlayerPaddle = new GameObject("DummyPlayerPaddle");
        GameObject dummyComputerPaddle = new GameObject("DummyComputerPaddle");
        GameObject dummyBall = new GameObject("DummyBall");

        //Rigidbody is assigned to them to avoid NullPointerException when the Paddle tries to access them.
        dummyPlayerPaddle.AddComponent<Rigidbody2D>();
        dummyComputerPaddle.AddComponent<Rigidbody2D>();
        dummyBall.AddComponent<Rigidbody2D>();

        // Assign the dummy objects to the GameManager
        gm.playerPaddle = dummyPlayerPaddle.AddComponent<Paddle>();
        gm.computerPaddle = dummyComputerPaddle.AddComponent<Paddle>();
        gm.ball = dummyBall.AddComponent<Ball>();

        _eventFired = false;

        // Subscribe our fake listener to the event
        GameManager.OnScoreUpdated += MockScoreListener;

        // 2. ACT: Force the player to score
        gm.PlayerScore();

        // 3. ASSERT: Prove that the code did what it was supposed to do
        Assert.IsTrue(_eventFired, "The score event did not fire!");
        Assert.AreEqual(1, _recordedPlayerIndex, "The event broadcasted the wrong player index!");
        Assert.AreEqual(1, _recordedScore, "The score did not increment to 1!");

        // 4. CLEANUP: Destroy the object and unsubscribe so it doesn't affect other tests
        GameManager.OnScoreUpdated -= MockScoreListener;
        Object.DestroyImmediate(gmObject);
        Object.DestroyImmediate(dummyPlayerPaddle);
        Object.DestroyImmediate(dummyComputerPaddle);
        Object.DestroyImmediate(dummyBall);
    }
    [Test]
    public void ComputerScore_IncrementScore_AndFiresEvent()
    {
        // 1. Arrangen : Set Up the environment
        GameObject gmObject = new GameObject("TestGameManager");
        GameManager gm = gmObject.AddComponent<GameManager>();

        //Creating Dummy game objects for ball, PlayerPaddle, CoputerPaddle.To pass the NullPointerException.
        GameObject dummyPlayerPaddle = new GameObject("DummyPlayerPaddle");
        GameObject dummyComputerPaddle = new GameObject("DummyComputerPaddle");
        GameObject dummyBall = new GameObject("DummyBall");

        //RigidBody Assigment
        dummyPlayerPaddle.AddComponent<Rigidbody2D>();
        dummyComputerPaddle.AddComponent<Rigidbody2D>();
        dummyBall.AddComponent<Rigidbody2D>();

        // Assign the dummy objects to the GameManager
        gm.playerPaddle = dummyPlayerPaddle.AddComponent<Paddle>();
        gm.computerPaddle = dummyComputerPaddle.AddComponent<Paddle>();
        gm.ball = dummyBall.AddComponent<Ball>();

        _eventFired = false;

        // Subscribe our fake listener to the event
        GameManager.OnScoreUpdated += MockScoreListener;

        // 2. ACT: Force the computer to score
        gm.ComputerScore();

        // 3. ASSERT: Prove that the code did what it was supposed to do
        Assert.IsTrue(_eventFired, "The Score event didn't Fire!");
        Assert.AreEqual(2, _recordedPlayerIndex, "The event broadcasted the wrong player index!");
        Assert.AreEqual(1, _recordedScore, "The score did not increment to 1!");

        // 4. CLEANUP: Destroy the object and unsubscribe so it doesn't affect other tests
        GameManager.OnScoreUpdated -= MockScoreListener;
        Object.DestroyImmediate(gmObject);
        Object.DestroyImmediate(dummyPlayerPaddle);
        Object.DestroyImmediate(dummyComputerPaddle);
        Object.DestroyImmediate(dummyBall);
    }

}   