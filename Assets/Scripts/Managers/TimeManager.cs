using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }
    private bool alteringTimescale = false;
    private float alterTimer = 0f;
    private float alterDuration = 1.5f;

    public EventHandler<bool> OnAlteringTimescale;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        if (alteringTimescale)
        {
            AlterTimeScale();
        }
    }

    private void AlterTimeScale()
    {
        if (alterTimer > alterDuration)
        {
            return;
        }

        alterTimer += Time.unscaledDeltaTime;

        float alterLerp = alterTimer / alterDuration;
        Time.timeScale = Mathf.Clamp01(Mathf.Lerp(1f, 0f, alterLerp));
    }

    public void PauseGame()
    {
        if (alteringTimescale)
        {
            return;
        }

        Time.timeScale = 0f;
    }

    public void UnpauseGame()
    {
        if (alteringTimescale)
        {
            return;
        }

        Time.timeScale = 1f;
    }

    public void GradualPause()
    {
        Time.timeScale = 1f;
        alteringTimescale = true;

        OnAlteringTimescale?.Invoke(this, alteringTimescale);
    }

    public void RestartTimeAfterGradualPause()
    {
        Time.timeScale = 1f;
        alteringTimescale = false;
        OnAlteringTimescale?.Invoke(this, alteringTimescale);
    }
}
