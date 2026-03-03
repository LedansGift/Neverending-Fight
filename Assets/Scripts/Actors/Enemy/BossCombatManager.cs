using UnityEngine;

public class BossCombatManager : MonoBehaviour
{
    private int attackPatternIndex = 0;

    [SerializeField]
    private Transform bossTransform;

    [SerializeField]
    private BossAttackNode[] bossAttackPattern;

    public void StartBossCombat()
    {
        attackPatternIndex = 0;
        PerformNextAttack();
    }

    private void PerformNextAttack()
    {
        BossAttackNode attack = bossAttackPattern[attackPatternIndex];
        attack.PerformAttack(bossTransform, ResolveAttack);
    }

    private void ResolveAttack()
    {
        //Debug.Log("Attack Finished");

        attackPatternIndex++;

        if (attackPatternIndex >= bossAttackPattern.Length)
        {
            attackPatternIndex = 0;
        }

        PerformNextAttack();
    }
}
