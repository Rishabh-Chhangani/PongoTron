using System;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScoringZone : MonoBehaviour
{

    public event Action<ScoringZone> OnBallScored;
    private void OnCollisionEnter2D(Collision2D collision)
    {

        Ball ball = collision.gameObject.GetComponent<Ball>();

        if (ball != null)
        {
            Debug.Log("Ball entered scoring zone!");
            OnBallScored?.Invoke(this);
        }
    }
}   