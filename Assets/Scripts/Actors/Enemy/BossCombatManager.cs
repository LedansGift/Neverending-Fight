using UnityEngine;

public class BossCombatManager : MonoBehaviour
{
    private int attackPatternIndex = 0;

    private BossAttackNode[] activeAttackPattern;

    [SerializeField]
    private BossAttackManager bossAttacker;

    public void StartBossCombat(BossAttackManager bossAttacker, BossAttackNode[] newAttackPattern)
    {
        attackPatternIndex = 0;
        activeAttackPattern = newAttackPattern;
        this.bossAttacker = bossAttacker;

        if (!this.bossAttacker || (activeAttackPattern == null))
        {
            return;
        }

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

        attackPatternIndex++;

        if (attackPatternIndex >= activeAttackPattern.Length)
        {
            attackPatternIndex = 0;
        }

        PerformNextAttack();
    }
}
