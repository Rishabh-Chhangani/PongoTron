using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [Tooltip("Set 1 for Player, 2 for Computer")]
    [SerializeField]
    private int playerIndex = 1;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private Animator animator;



    private void OnEnable()
    {
        GameManager.OnScoreUpdated += HandleScoreUpdated;
    }
    private void OnDisable()
    {
        GameManager.OnScoreUpdated -= HandleScoreUpdated;
    }


    private void HandleScoreUpdated(int player, int newScore)
    {
        if(player == playerIndex)
        {
            scoreText.text = newScore.ToString();

            if(animator != null)
            {
                animator.SetTrigger("highlight");
            }
        }
    }
}
