using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class AudioPlayModeTests
{
    private GameObject _audioObject;
    private AudioSource _audioSource;
    private AudioClip _testClip;

    [SetUp]
    public void SetUp()
    {
        _testClip = AudioClip.Create("TestClip", 44100, 1, 44100, false);
    }

    [TearDown]
    public void TearDown()
    {
        if (_audioObject != null) Object.Destroy(_audioObject);
        if (_testClip != null) Object.Destroy(_testClip);
    }

    [UnityTest]
    public IEnumerator BallAudioListener_OnBallCollidedPaddle_TriggersPaddleAudio()
    {
        // Arrange
        _audioObject = new GameObject("BallAudioObject");
        _audioSource = _audioObject.AddComponent<AudioSource>();
        BallAudio ballAudio = _audioObject.AddComponent<BallAudio>();
        ballAudio.asSounds = _audioSource;
        ballAudio.paddleSound = _testClip;
        ballAudio.wallSound = _testClip;

        BallAudioListner listener = _audioObject.AddComponent<BallAudioListner>();

        // Act: invoke private handler for Paddle hit
        MethodInfo method = typeof(BallAudioListner).GetMethod("PlayBallHitSound", BindingFlags.NonPublic | BindingFlags.Instance);
        method.Invoke(listener, new object[] { Ball.HitType.Paddle });
        yield return null;

        // Assert: AudioSource is playing clip or triggered
        Assert.IsTrue(_audioSource.isPlaying, "AudioSource should be playing after Ball collides with Paddle.");
    }

    [UnityTest]
    public IEnumerator BallAudioListener_OnBallCollidedWall_TriggersWallAudio()
    {
        // Arrange
        _audioObject = new GameObject("BallAudioObject");
        _audioSource = _audioObject.AddComponent<AudioSource>();
        BallAudio ballAudio = _audioObject.AddComponent<BallAudio>();
        ballAudio.asSounds = _audioSource;
        ballAudio.wallSound = _testClip;

        BallAudioListner listener = _audioObject.AddComponent<BallAudioListner>();

        // Act: invoke private handler for Wall hit
        MethodInfo method = typeof(BallAudioListner).GetMethod("PlayBallHitSound", BindingFlags.NonPublic | BindingFlags.Instance);
        method.Invoke(listener, new object[] { Ball.HitType.Wall });
        yield return null;

        // Assert
        Assert.IsTrue(_audioSource.isPlaying, "AudioSource should be playing after Ball collides with Wall.");
    }

    [UnityTest]
    public IEnumerator GameAudioListener_OnScoreUpdated_TriggersScoreAudio()
    {
        // Arrange
        _audioObject = new GameObject("GameAudioObject");
        _audioSource = _audioObject.AddComponent<AudioSource>();
        GameAudio gameAudio = _audioObject.AddComponent<GameAudio>();
        gameAudio.asSounds = _audioSource;
        gameAudio.scoreSound = _testClip;
        gameAudio.winSound = _testClip;

        GameAudioListner listener = _audioObject.AddComponent<GameAudioListner>();

        // Act: invoke private handler for Score Sound
        MethodInfo method = typeof(GameAudioListner).GetMethod("HandleScoreSound", BindingFlags.NonPublic | BindingFlags.Instance);
        method.Invoke(listener, new object[] { 1, 1 });
        yield return null;

        // Assert
        Assert.IsTrue(_audioSource.isPlaying, "AudioSource should be playing score sound.");
    }

    [UnityTest]
    public IEnumerator GameAudioListener_OnGameWon_TriggersWinAudio()
    {
        // Arrange
        _audioObject = new GameObject("GameAudioObject");
        _audioSource = _audioObject.AddComponent<AudioSource>();
        GameAudio gameAudio = _audioObject.AddComponent<GameAudio>();
        gameAudio.asSounds = _audioSource;
        gameAudio.winSound = _testClip;

        GameAudioListner listener = _audioObject.AddComponent<GameAudioListner>();

        // Act: invoke private handler for Win Sound
        MethodInfo method = typeof(GameAudioListner).GetMethod("HandleWinSound", BindingFlags.NonPublic | BindingFlags.Instance);
        method.Invoke(listener, new object[] { 1 });
        yield return null;

        // Assert
        Assert.IsTrue(_audioSource.isPlaying, "AudioSource should be playing win sound.");
    }
}
