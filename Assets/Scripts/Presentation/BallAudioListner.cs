using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAudioListner : MonoBehaviour
{
    private BallAudio ballAudio;
    private void OnEnable()
    {
        Ball.OnBallCollided += PlayBallHitSound;
    }
    private void OnDisable()
    {
        Ball.OnBallCollided -= PlayBallHitSound;
    }

    private void Awake()
    {
        if (ballAudio == null)
        {
            ballAudio = GetComponent<BallAudio>();
        }
    }

    private void PlayBallHitSound(Ball.HitType hitType)
    {
        if(hitType == Ball.HitType.Paddle)
        {
            ballAudio.PlayPaddleSound();
        }
        else if(hitType == Ball.HitType.Wall)
        {
            ballAudio.PlayWallSound();
        }
    }
}
