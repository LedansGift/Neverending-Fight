using System.Collections;
using UnityEngine;

public class BossCombatManager : MonoBehaviour
{
    private int attackPatternIndex = 0;
    private BossFormManager bossFormManager;
    private BossNode[] activeAttackPattern;
    private HealthThresholdPhaseChange activeHealthPhaseChange;
    private BattleStatePhaseChange activeBattleStatePhaseChange;

    [SerializeField]
    private BossAttackManager bossAttacker;

    [SerializeField]
    private BossHealth bossHealth;

    public void StartBossCombat(
        BossAttackManager bossAttacker,
        BossNode[] newAttackPattern,
        HealthThresholdPhaseChange healthPhaseChange = null,
        BattleStatePhaseChange battleStatePhaseChange = null
    )
    {
        attackPatternIndex = 0;
        activeAttackPattern = newAttackPattern;
        activeHealthPhaseChange = healthPhaseChange;
        activeBattleStatePhaseChange = battleStatePhaseChange;

        if (bossAttacker)
        {
            this.bossAttacker = bossAttacker;
        }

        if (!this.bossAttacker || (activeAttackPattern == null))
        {
            return;
        }

        BossCastBarUI.CancelCast();

        StartCoroutine(DelayedAttacksStart());
    }

    private IEnumerator DelayedAttacksStart()
    {
        yield return new WaitForSeconds(1f);

        PerformNextAttack();
    }

    private void PerformNextAttack()
    {
        BossNode currentAttack = activeAttackPattern[attackPatternIndex];
        bossAttacker.PerformAttackNode(currentAttack, ResolveAttack);
    }

    private void ResolveAttack()
    {
        //Debug.Log("Attack Finished");
        if (
            (activeBattleStatePhaseChange != null)
            && activeBattleStatePhaseChange.ResolveBattleState()
        )
        {
            StopAllCoroutines();

            bossFormManager.InitiateMidFightPhaseChange(activeBattleStatePhaseChange.GetNewPhase());

            Debug.Log("BATTLE STATE PHASE CHANGE");

            return;
        }
        else if (
            (activeHealthPhaseChange != null)
            && (bossHealth.GetHealthPercentage() <= activeHealthPhaseChange.GetHealthThreshold())
        )
        {
            StopAllCoroutines();
            //activeHealthPhaseChange.GetNewPhase().InitialiseBossPhase(bossFormManager);
            bossFormManager.InitiateMidFightPhaseChange(activeHealthPhaseChange.GetNewPhase());

            Debug.Log("HEALTH-PHASE CHANGE");

            return;
        }

        attackPatternIndex++;

        if (attackPatternIndex >= activeAttackPattern.Length)
        {
            attackPatternIndex = 0;
        }

        PerformNextAttack();
    }

    public void SetFormManager(BossFormManager bossFormManager)
    {
        this.bossFormManager = bossFormManager;
    }
}
