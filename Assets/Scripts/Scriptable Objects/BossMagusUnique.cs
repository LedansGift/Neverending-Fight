using System;
using UnityEngine;

[CreateAssetMenu(
    fileName = "Boss Unique Attack",
    menuName = "Boss Attack/Unique Attack/Magus",
    order = 4
)]
public class BossMagusUnique : BossUniqueAttack
{
    [SerializeField]
    private MagusUniqueAttacks magusUniqueAttack;

    public override void PerformAttack(
        BossAttackManager attacker,
        Action OnAttackFinished,
        float damageMultiplier = 1
    )
    {
        uniqueAttackIndex = (int)magusUniqueAttack;

        base.PerformAttack(attacker, OnAttackFinished, damageMultiplier);
    }
}
