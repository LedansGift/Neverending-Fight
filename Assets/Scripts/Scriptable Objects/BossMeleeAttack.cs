using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Boss Melee Attack", menuName = "Boss Attack/Melee Attack", order = 1)]
public class BossMeleeAttack : BossAttackNode
{
    public override void PerformAttack(Transform attackTransform, Action OnAttackFinished)
    {
        this.OnAttackFinished = OnAttackFinished;
    }
}
