using System;
using UnityEngine;

public class PlayerTimepiece : MonoBehaviour
{
    private int playerRetries = 0;
    private const int MAX_PLAYER_RETRIES = 2;

    public static Action OnNoMoreRetries;
    public static Action OnPlayerRetry;
    public static EventHandler<int> OnNewPlayerRetries;

    private void Start()
    {
        // If first time, do not setup player retries
        ResetPlayerRetries();
    }

    private void UpdateRetryUI()
    {
        OnNewPlayerRetries?.Invoke(this, playerRetries);
    }

    public void DecrementRetries()
    {
        if (playerRetries <= 0)
        {
            OnNoMoreRetries?.Invoke();
            return;
        }

        playerRetries--;
        OnPlayerRetry?.Invoke();

        UpdateRetryUI();
    }

    public void ResetPlayerRetries()
    {
        playerRetries = MAX_PLAYER_RETRIES;
        UpdateRetryUI();
    }

    public int GetAvailableRetries()
    {
        return playerRetries;
    }
}
