using System;
using UnityEngine;

[CreateAssetMenu(
    fileName = "Boss Complex Attack",
    menuName = "Boss Attack/Complex Attack",
    order = 4
)]
public class BossComplexAttack : BossAttackNode
{
    private int nodeIndex = 0;

    [SerializeField]
    private BossAttackNode[] attackNodes;

    [SerializeField]
    private BossIdleNode finalNode;

    private BossAttackManager attackManager;
    private float damageMultiplier = 1f;

    public override void PerformAttack(
        BossAttackManager attacker,
        Action OnAttackFinished,
        float damageMultiplier = 1
    )
    {
        this.OnAttackFinished = OnAttackFinished;
        nodeIndex = 0;

        TryPerformNode(attacker, damageMultiplier);
    }

    private void TryPerformNode(BossAttackManager attacker, float damageMultiplier = 1)
    {
        if (nodeIndex >= attackNodes.Length)
        {
            finalNode.PerformAttack(attacker, FinishComplexAttack);
            return;
        }

        BossAttackNode activeNode = attackNodes[nodeIndex];
        nodeIndex++;

        if (activeNode.GetType() == typeof(BossIdleNode))
        {
            attackManager = attacker;
            this.damageMultiplier = damageMultiplier;
            activeNode.PerformAttack(attacker, NodeDelay);
        }
        else if (activeNode.GetType() == typeof(BossMeleeAttack))
        {
            BossMeleeAttack meleeNode = activeNode as BossMeleeAttack;
            meleeNode.PerformAttackPart(attacker, FinishAttack, damageMultiplier);
            TryPerformNode(attacker, damageMultiplier);
        }
        else
        {
            activeNode.PerformAttack(attacker, null, damageMultiplier);
            TryPerformNode(attacker, damageMultiplier);
        }
    }

    public override void FinishAttack() { }

    private void FinishComplexAttack()
    {
        OnAttackFailCheck?.Invoke(this, EventArgs.Empty);

        OnAttackFinished();
    }

    private void NodeDelay()
    {
        TryPerformNode(attackManager, damageMultiplier);
    }
}
