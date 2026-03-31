using UnityEngine;

public class BossCombatManager : MonoBehaviour
{
    private int attackPatternIndex = 0;

    private BossAttackNode[] activeAttackPattern;

    // [SerializeField]
    // private Transform bossTransform;

    [SerializeField]
    private BossAttackManager bossAttacker;

    public void StartBossCombat(BossAttackNode[] newAttackPattern)
    {
        attackPatternIndex = 0;
        activeAttackPattern = newAttackPattern;

        if (activeAttackPattern == null)
        {
            return;
        }

        PerformNextAttack();
    }

    private void PerformNextAttack()
    {
        BossAttackNode currentAttack = activeAttackPattern[attackPatternIndex];
        //currentAttack.PerformAttack(bossAttacker, ResolveAttack);
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
