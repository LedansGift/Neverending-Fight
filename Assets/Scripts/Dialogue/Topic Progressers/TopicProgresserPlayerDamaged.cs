using System;
using UnityEngine;

public class TopicProgresserPlayerDamaged : TopicProgresser
{
    private bool playerHit = false;

    [SerializeField]
    private Dialogue firstHitDialogue;

    private void OnEnable()
    {
        playerHit = false;

        PlayerHealth.OnPlayerHit += TryProgressTopic;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerHit -= TryProgressTopic;
    }

    private void TryProgressTopic()
    {
        if (playerHit)
        {
            ProgressTopic();
        }
        else
        {
            MonologueManager.Instance.AddToConversation(firstHitDialogue);
            playerHit = true;
        }
    }
}
