using System;
using System.Collections;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;
    private bool canPause = true;

    public static EventHandler<bool> OnPauseGame;

    private void Awake()
    {
        canPause = true;
        isPaused = false;
    }

    private void OnEnable()
    {
        StartCoroutine(DelayedEnable());
    }

    private IEnumerator DelayedEnable()
    {
        yield return null;
        InputManager.Instance.OnPauseEvent += TryPauseGame;
        TimeManager.Instance.OnAlteringTimescale += ToggleCanPause;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnPauseEvent -= TryPauseGame;
        TimeManager.Instance.OnAlteringTimescale -= ToggleCanPause;
    }

    private void TogglePauseGame(bool toggle)
    {
        if (toggle)
        {
            TimeManager.Instance.PauseGame();
        }
        else
        {
            TimeManager.Instance.UnpauseGame();
        }

        InputManager.Instance.ToggleDisableInputs(this, toggle);

        OnPauseGame?.Invoke(this, toggle);
    }

    public void TryPauseGame()
    {
        if (!canPause || !TimeManager.Instance.CanPauseGame())
        {
            return;
        }

        isPaused = !isPaused;
        TogglePauseGame(isPaused);
    }

    private void ToggleCanPause(object sender, bool alteringTimescale)
    {
        canPause = !alteringTimescale;

        isPaused = false;
        OnPauseGame?.Invoke(this, false);
    }
}
