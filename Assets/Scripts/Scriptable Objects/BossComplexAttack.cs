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
    private BossNode[] attackNodes;

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
        attackManager = attacker;

        PlayFX(attacker.transform);

        TryPerformNode(attacker, damageMultiplier);
    }

    private void TryPerformNode(BossAttackManager attacker, float damageMultiplier = 1)
    {
        if (nodeIndex >= attackNodes.Length)
        {
            finalNode.PerformAttack(attacker, FinishComplexAttack);
            return;
        }

        BossNode activeNode = attackNodes[nodeIndex];
        nodeIndex++;

        if (activeNode.GetType() == typeof(BossIdleNode))
        {
            this.damageMultiplier = damageMultiplier;
            activeNode.PerformAttack(attacker, NodeDelay);
        }
        else if (activeNode.GetType() == typeof(BossMeleeAttack))
        {
            BossMeleeAttack meleeNode = activeNode as BossMeleeAttack;
            meleeNode.PerformAttackPart(attacker, NodeDelay, damageMultiplier, GetAttackName());
            //TryPerformNode(attacker, damageMultiplier);
        }
        else if (activeNode.GetType() == typeof(BossRangedAttack))
        {
            BossRangedAttack rangedNode = activeNode as BossRangedAttack;
            rangedNode.PerformAttackPart(attacker, NodeDelay, GetAttackName(), damageMultiplier);
        }
        else
        {
            activeNode.PerformAttack(attacker, NodeDelay, damageMultiplier);
            //TryPerformNode(attacker, damageMultiplier);
        }
    }

    public override void FinishAttack() { }

    private void FinishComplexAttack()
    {
        if (failableAttack)
        {
            OnAttackFailCheck?.Invoke(this, GetAttackName());
        }

        OnAttackFinished();
    }

    private void NodeDelay()
    {
        TryPerformNode(attackManager, damageMultiplier);
    }
}
