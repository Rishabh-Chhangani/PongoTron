using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudio : MonoBehaviour
{

    public AudioSource asSounds;

   
    public AudioClip winSound;
    public AudioClip scoreSound;

    public void PlayWinSound()
    {
        asSounds.PlayOneShot(winSound);
    }
    public void PlayScoreSound()
    {
        asSounds.PlayOneShot(scoreSound);
    }
}
