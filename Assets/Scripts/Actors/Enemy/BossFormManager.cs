using System;
using System.Collections;
using UnityEngine;

public class BossFormManager : MonoBehaviour
{
    private bool bossActive = false;
    private bool bossFormActive = false;

    [SerializeField]
    private Animator bossAnimator;

    [SerializeField]
    private BossHealth bossHealth;

    [SerializeField]
    private BossMover bossMover;

    [SerializeField]
    private BossPhaseManager bossPhaseManager;

    [SerializeField]
    private BossCombatManager bossCombatManager;

    [SerializeField]
    private BossAttackManager bossAttackManager;

    public Action OnFinalPhaseFinished;
    public static Action OnPhaseFinished;
    public static Action OnNewPhaseStart;
    public static EventHandler<Action> OnPhaseChange;

    private void OnEnable()
    {
        bossHealth.OnDeath += HandleBossDeath;
        RestartManager.OnResetPhase += ResetBossPhase;
    }

    private void OnDisable()
    {
        bossHealth.OnDeath -= HandleBossDeath;
        RestartManager.OnResetPhase -= ResetBossPhase;
    }

    private void InitialiseBoss()
    {
        bossMover.ResetMover();

        if (!bossPhaseManager.TryGetPhase(out BossPhase phase))
        {
            return;
        }

        bossHealth.InitialiseHealth();
        BossCastBarUI.CancelCast();

        bossCombatManager.StartBossCombat(bossAttackManager, phase.GetAttackPattern());
        bossActive = true;
    }

    private void HandleBossDeath()
    {
        if (!bossActive)
        {
            return;
        }

        bossActive = false;
        bossAttackManager.PhaseEndCleanup();
        OnPhaseFinished?.Invoke();

        //InitiatePhaseChange();
        OnPhaseChange?.Invoke(this, InitiatePhaseChange);
    }

    private void InitiatePhaseChange()
    {
        bossPhaseManager.AdvancePhaseTracker();

        if (bossPhaseManager.TryGetPhase(out BossPhase phase))
        {
            //Start phase change cutscene that callbacks to Initialise Boss
            OnNewPhaseStart?.Invoke();
            InitialiseBoss();
        }
        else
        {
            //Start phase change cutscene that callbacks to finishing the final phase
            Debug.Log("Final Phase Finished");
            OnFinalPhaseFinished?.Invoke();
        }
    }

    public void ActivateBossForm()
    {
        bossFormActive = true;
        OnNewPhaseStart?.Invoke();
        InitialiseBoss();
    }

    public void DeactivateBossForm()
    {
        if (bossActive)
        {
            bossActive = false;
            bossAttackManager.PhaseEndCleanup();
            OnPhaseFinished?.Invoke();
        }

        bossFormActive = false;

        //Definitelt needs changing later
        gameObject.SetActive(false);
    }

    public void PlayBossDamagedAnimation()
    {
        bossAnimator.SetTrigger("bigdamage");
    }

    public BossAttackManager GetBossAttackManager()
    {
        return bossAttackManager;
    }

    // private void FinalizePhaseChange()
    // {

    // }

    private IEnumerator DelayedBossReset()
    {
        yield return null;

        InitialiseBoss();
    }

    private void ResetBossPhase()
    {
        if (!bossFormActive)
        {
            return;
        }

        StartCoroutine(DelayedBossReset());
    }
}
