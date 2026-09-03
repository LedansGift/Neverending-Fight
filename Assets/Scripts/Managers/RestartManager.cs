using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartManager : MonoBehaviour
{
    private float phaseResetStartupDuration = 2f;
    private float phaseResetVisualDuration = 6f;
    private float levelResetVisualDuration = 2f;

    private float canisterDepleteTime = 2.5f;

    [SerializeField]
    private GameObject restartVisualObjects;

    [SerializeField]
    private Animator restartVisualAnimator;

    [SerializeField]
    private TimepieceCanister timepieceCanister1;

    [SerializeField]
    private TimepieceCanister timepieceCanister2;

    [SerializeField]
    private SFXObject rewindSFX;

    public static Action OnStartPhaseResetVisual;
    public static Action OnResetPhase;
    public static Action OnStartLevelResetVisual;
    public static EventHandler<bool> OnToggleRewindUI;

    private void Start()
    {
        restartVisualObjects.SetActive(true);
    }

    private void OnEnable()
    {
        PlayerTimepiece.OnPlayerRetry += ResetPhase;
        PlayerTimepiece.OnNoMoreRetries += ResetLevel;
        PlayerTimepiece.OnResetRetries += ResetRetries;
    }

    private void OnDisable()
    {
        PlayerTimepiece.OnPlayerRetry -= ResetPhase;
        PlayerTimepiece.OnNoMoreRetries -= ResetLevel;
        PlayerTimepiece.OnResetRetries -= ResetRetries;
    }

    private void ResetPhase(object sender, int retriesRemaining)
    {
        OnStartPhaseResetVisual?.Invoke();
        TimeManager.Instance.GradualPause();

        StartCoroutine(DelayedPhaseReset(retriesRemaining));
    }

    private IEnumerator DelayedPhaseReset(int retriesRemaining)
    {
        //Play animation of screen darkening and timepiece rewinding

        yield return new WaitForSecondsRealtime(phaseResetStartupDuration);

        restartVisualAnimator.SetTrigger("rewind");

        rewindSFX?.PlaySFX(transform.position, false);

        OnToggleRewindUI?.Invoke(this, true);

        yield return new WaitForSecondsRealtime(canisterDepleteTime);

        if (retriesRemaining >= 1)
        {
            timepieceCanister2.DisableCanister();
        }
        else
        {
            timepieceCanister1.DisableCanister();
        }

        yield return new WaitForSecondsRealtime(phaseResetVisualDuration - canisterDepleteTime);
        OnResetPhase?.Invoke();

        OnToggleRewindUI?.Invoke(this, false);

        TimeManager.Instance.RestartTimeAfterGradualPause();
    }

    private void ResetLevel()
    {
        OnStartLevelResetVisual?.Invoke();

        if (BossManager.Instance.GetActiveBossForm() == BossForm.CROSSROADS)
        {
            BossManager.Instance.ActivateTutorialBossForm();

            return;
        }

        StartCoroutine(DelayedLevelReset());
    }

    private IEnumerator DelayedLevelReset()
    {
        yield return new WaitForSecondsRealtime(levelResetVisualDuration);
        // Reset level via LevelManager that handles all necessary load screens and the like
        //Temp
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ResetRetries()
    {
        timepieceCanister1.ReplenishCanister();
        timepieceCanister2.ReplenishCanister();
    }
}
