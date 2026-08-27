using System;
using UnityEngine;

[CreateAssetMenu(
    fileName = "Boss Unique Attack",
    menuName = "Boss Attack/Unique Attack/Magpie",
    order = 1
)]
public class BossMagpieUnique : BossUniqueAttack
{
    [SerializeField]
    private MagpieUniqueAttacks magpieUniqueAttack;

    public override void PerformAttack(
        BossAttackManager attacker,
        Action OnAttackFinished,
        float damageMultiplier = 1
    )
    {
        uniqueAttackIndex = (int)magpieUniqueAttack;

        base.PerformAttack(attacker, OnAttackFinished, damageMultiplier);
    }
}
