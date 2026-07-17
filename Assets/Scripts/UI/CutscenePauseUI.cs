using System;
using UnityEngine;
using UnityEngine.UI;

public class CutscenePauseUI : MonoBehaviour
{
    private bool cutsceneActive = false;

    private bool pauseActive = false;
    private bool skipTimerActive = false;
    private float skipTimer = 0f;
    private float skipDuration = 2f;

    private PauseManager pauseManager;

    [SerializeField]
    private Slider skipSlider;

    [SerializeField]
    private CanvasGroupFader pauseFader;

    private void Start()
    {
        cutsceneActive = false;

        CutsceneManager.Instance.OnCutsceneStart += SetCutsceneStarted;
        CutsceneManager.Instance.OnCutsceneEnd += SetCutsceneEnded;

        PauseManager.OnPauseGame += TogglePauseUI;
        InputManager.Instance.OnSkipEvent += StartSkipTimer;
        InputManager.Instance.OnSkipReleaseEvent += EndSkipTimer;

        pauseFader.SetCanvasGroupAlpha(0f);
    }

    private void OnDisable()
    {
        CutsceneManager.Instance.OnCutsceneStart -= SetCutsceneStarted;
        CutsceneManager.Instance.OnCutsceneEnd -= SetCutsceneEnded;

        PauseManager.OnPauseGame -= TogglePauseUI;
        InputManager.Instance.OnSkipEvent -= StartSkipTimer;
        InputManager.Instance.OnSkipReleaseEvent -= EndSkipTimer;
    }

    private void Update()
    {
        if (skipTimerActive)
        {
            skipTimer += Time.unscaledDeltaTime;

            skipSlider.value = skipTimer / skipDuration;

            if (skipTimer >= skipDuration)
            {
                SkipCutscene();
            }
        }
    }

    private void SkipCutscene()
    {
        CutsceneManager.Instance.SkipActiveCutscene();

        EndSkipTimer();
        pauseManager.TryPauseGame();
    }

    private void StartSkipTimer()
    {
        if (!pauseActive)
        {
            return;
        }

        skipTimer = 0f;
        skipSlider.value = 0f;
        skipTimerActive = true;
    }

    private void EndSkipTimer()
    {
        if (!pauseActive)
        {
            return;
        }

        skipTimer = 0f;
        skipSlider.value = 0f;
        skipTimerActive = false;
    }

    private void TogglePauseUI(object sender, bool toggle)
    {
        if (!cutsceneActive)
        {
            return;
        }

        if (!pauseManager)
        {
            pauseManager = sender as PauseManager;
        }

        pauseActive = !pauseActive;
        pauseFader.ToggleFade(pauseActive);
    }

    private void SetCutsceneStarted()
    {
        cutsceneActive = true;
    }

    private void SetCutsceneEnded()
    {
        cutsceneActive = false;

        if (pauseActive)
        {
            pauseActive = false;
            pauseFader.ToggleFade(pauseActive);
        }
    }
}
