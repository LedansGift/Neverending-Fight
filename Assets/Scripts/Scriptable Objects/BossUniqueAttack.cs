using System;
using UnityEngine;

[CreateAssetMenu(
    fileName = "Boss Unique Attack",
    menuName = "Boss Attack/Unique Attack",
    order = 6
)]
public class BossUniqueAttack : BossAttackNode
{
    [SerializeField]
    private string uniqueAttackIdentifier;

    public override void PerformAttack(
        BossAttackManager attacker,
        Action OnAttackFinished,
        float damageMultiplier = 1
    )
    {
        this.OnAttackFinished = OnAttackFinished;
        //initiate unique attack via state change
    }

    public override void FinishAttack()
    {
        OnAttackFailCheck?.Invoke(this, EventArgs.Empty);

        base.FinishAttack();
    }
}
