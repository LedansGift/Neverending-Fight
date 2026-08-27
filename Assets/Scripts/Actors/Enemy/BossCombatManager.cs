using System.Collections;
using UnityEngine;

public class BossCombatManager : MonoBehaviour
{
    private int attackPatternIndex = 0;
    private BossFormManager bossFormManager;
    private BossAttackNode[] activeAttackPattern;
    private HealthThresholdPhaseChange activeHealthPhaseChange;

    [SerializeField]
    private BossAttackManager bossAttacker;

    [SerializeField]
    private BossHealth bossHealth;

    public void StartBossCombat(
        BossAttackManager bossAttacker,
        BossAttackNode[] newAttackPattern,
        HealthThresholdPhaseChange healthPhaseChange = null
    )
    {
        attackPatternIndex = 0;
        activeAttackPattern = newAttackPattern;
        activeHealthPhaseChange = healthPhaseChange;

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
        BossAttackNode currentAttack = activeAttackPattern[attackPatternIndex];
        bossAttacker.PerformAttackNode(currentAttack, ResolveAttack);
    }

    private void ResolveAttack()
    {
        //Debug.Log("Attack Finished");

        if (
            (activeHealthPhaseChange != null)
            && (bossHealth.GetHealthPercentage() <= activeHealthPhaseChange.GetHealthThreshold())
        )
        {
            activeHealthPhaseChange.GetNewPhase().InitialiseBossPhase(bossFormManager);

            Debug.Log("HEALTH-PHASE CHANGE");

            StartBossCombat(
                bossAttacker,
                activeHealthPhaseChange.GetNewPhase().GetAttackPattern(),
                activeHealthPhaseChange.GetNewPhase().GetHealthPhaseChange()
            );
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
