using System;
using System.Collections;
using UnityEngine;

public class BossFormManager : MonoBehaviour
{
    private bool bossActive = false;

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

    public void InitialiseBoss()
    {
        bossHealth.InitialiseHealth();
        bossMover.ResetMover();

        if (!bossPhaseManager.TryGetPhase(out BossPhase phase))
        {
            return;
        }

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

        InitiatePhaseChange();
    }

    private void InitiatePhaseChange()
    {
        bossPhaseManager.AdvancePhaseTracker();

        if (bossPhaseManager.TryGetPhase(out BossPhase phase))
        {
            //Start phase change cutscene that callbacks to Initialise Boss
            InitialiseBoss();
        }
        else
        {
            //Start phase change cutscene that callbacks to finishing the final phase
            Debug.Log("Final Phase Finished");
            OnFinalPhaseFinished?.Invoke();
        }
    }

    private IEnumerator DelayedBossReset()
    {
        yield return null;

        InitialiseBoss();
    }

    private void ResetBossPhase()
    {
        StartCoroutine(DelayedBossReset());
    }
}
