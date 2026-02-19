using System;
using UnityEngine;
using UnityEngine.Rendering;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }
    private bool alteringTimescale = false;
    private float alterTimer = 0f;
    private float alterDuration = 1.5f;

    [SerializeField]
    private Volume timeStopVolume;

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

    private void Start()
    {
        Time.timeScale = 1f;
        timeStopVolume.weight = 0f;
        alteringTimescale = false;
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
        float lerpProgression = Mathf.Clamp01(Mathf.Lerp(1f, 0f, alterLerp));
        Time.timeScale = lerpProgression;
        timeStopVolume.weight = 1f - lerpProgression;
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
        alterTimer = 0f;
        alteringTimescale = true;

        OnAlteringTimescale?.Invoke(this, alteringTimescale);
    }

    public void RestartTimeAfterGradualPause()
    {
        Time.timeScale = 1f;
        timeStopVolume.weight = 0f;
        alteringTimescale = false;
        OnAlteringTimescale?.Invoke(this, alteringTimescale);
    }
}
