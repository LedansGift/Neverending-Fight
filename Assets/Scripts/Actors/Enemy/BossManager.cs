using System.Collections;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    [SerializeField]
    private BossHealth bossHealth;

    [SerializeField]
    private BossMover bossMover;

    [SerializeField]
    private BossCombatManager bossCombatManager;

    [SerializeField]
    private BossPhaseManager bossPhaseManager;

    private void Start()
    {
        //Temp
        InitialiseBoss();
    }

    private void OnEnable()
    {
        RestartManager.OnResetPhase += ResetBossPhase;
    }

    private void OnDisable()
    {
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

        bossCombatManager.StartBossCombat(phase.GetAttackPattern());
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
