using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartManager : MonoBehaviour
{
    private float phaseResetVisualDuration = 2f;
    private float levelResetVisualDuration = 2f;
    public static Action OnStartPhaseResetVisual;
    public static Action OnResetPhase;
    public static Action OnStartLevelResetVisual;

    private void OnEnable()
    {
        PlayerManager.OnNewPlayerRetries += ResetPhase;
        PlayerManager.OnNoMoreRetries += ResetLevel;
    }

    private void OnDisable()
    {
        PlayerManager.OnNewPlayerRetries -= ResetPhase;
        PlayerManager.OnNoMoreRetries -= ResetLevel;
    }

    private void ResetPhase(object sender, int e)
    {
        OnStartPhaseResetVisual?.Invoke();
        TimeManager.Instance.GradualPause();

        StartCoroutine(DelayedPhaseReset());
    }

    private IEnumerator DelayedPhaseReset()
    {
        yield return new WaitForSecondsRealtime(phaseResetVisualDuration);
        OnResetPhase?.Invoke();

        //Deactivate all projectiles
        //Set player location to start of phase
        //Reset all player resources
        //Set player active weapon
        //Reset boss health and position



        TimeManager.Instance.RestartTimeAfterGradualPause();
    }

    private void ResetLevel()
    {
        OnStartLevelResetVisual?.Invoke();

        StartCoroutine(DelayedLevelReset());
    }

    private IEnumerator DelayedLevelReset()
    {
        yield return new WaitForSecondsRealtime(levelResetVisualDuration);
        // Reset level via LevelManager that handles all necessary load screens and the like
        //Temp
        SceneManager.LoadScene(0);
    }
}
