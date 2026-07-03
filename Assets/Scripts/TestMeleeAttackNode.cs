using System;

[Serializable]
public class TestMeleeAttackNode : TestAttackNode
{
    public MeleeAttack[] attacks;

    // public override void PerformAttack(
    //     BossAttackManager attacker,
    //     Action OnAttackFinished,
    //     float damageMultiplier = 1f
    // )
    // {
    //     this.OnAttackFinished = OnAttackFinished;

    //     BossMeleeAttacker meleeAttacker = attacker.GetBossMeleeAttacker();
    //     meleeAttacker.PerformMeleeAttacks(attacks, damageMultiplier, FinishAttack);
    // }

    public void PerformAttack(
        BossAttackManager attacker,
        Action OnAttackFinished,
        float damageMultiplier = 1f
    )
    {
        BossMeleeAttacker meleeAttacker = attacker.GetBossMeleeAttacker();
        meleeAttacker.PerformMeleeAttacks(attacks, damageMultiplier, OnAttackFinished);
    }

    public override void FinishAttack(object sender, bool attackFailed)
    {
        // tell attack manager that a this attack was failed
        OnAttackFailCheck?.Invoke(this, attackFailed);

        base.FinishAttack(sender, attackFailed);
    }

    public MeleeAttack[] GetAttacks()
    {
        return attacks;
    }
}
