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

    [SerializeField]
    private BossTopicInitialiser bossTopicInitialiser;

    [SerializeField]
    private MusicTrack[] bossMusicTracks;

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

        //phase.InitialiseBossPhase(this);

        bossPhaseManager.SwitchPhase(phase);
        bossCombatManager.SetFormManager(this);

        bossTopicInitialiser.InitialiseTopics(bossPhaseManager.GetCurrentPhaseIndex());

        bossCombatManager.StartBossCombat(
            bossAttackManager,
            phase.GetAttackPattern(),
            phase.GetEndOfPatternPhaseChange(),
            phase.GetHealthPhaseChange(),
            phase.GetBattleStatePhaseChange()
        );
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

        OnPhaseChange?.Invoke(this, InitiateDeathPhaseChange);
    }

    private void InitiateDeathPhaseChange()
    {
        bossPhaseManager.AdvancePhaseTracker();

        TopicManager.Instance.AdvancePhase();

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

    public void InitiateMidFightPhaseChange(BossPhase newPhase)
    {
        BossCastBarUI.CancelCast();

        bossPhaseManager.SwitchPhase(newPhase);

        bossCombatManager.StartBossCombat(
            bossAttackManager,
            newPhase.GetAttackPattern(),
            newPhase.GetEndOfPatternPhaseChange(),
            newPhase.GetHealthPhaseChange(),
            newPhase.GetBattleStatePhaseChange()
        );
    }

    public void TryPlayNewPhaseMusic()
    {
        if ((bossPhaseManager.GetCurrentPhaseIndex() + 1) >= bossPhaseManager.GetTotalPhases())
        {
            return;
        }

        AudioManager.SetMusicTrack(bossMusicTracks?[bossPhaseManager.GetCurrentPhaseIndex() + 1]);
    }

    public void ActivateBossForm()
    {
        bossFormActive = true;
        bossAttackManager.ToggleAttackManager(true);
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
        bossAttackManager.ToggleAttackManager(false);
    }

    public void PlayBossDamagedAnimation()
    {
        bossAnimator.SetTrigger("bigdamage");
    }

    public BossAttackManager GetBossAttackManager()
    {
        return bossAttackManager;
    }

    private IEnumerator DelayedBossReset()
    {
        yield return null;

        TopicManager.Instance.ResetActiveTopics();
        bossPhaseManager.ResetCurrentPhase();

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
