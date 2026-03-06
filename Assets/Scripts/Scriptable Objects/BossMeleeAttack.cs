using System;
using UnityEngine;

[Serializable]
public struct MeleeAttack
{
    public int attackDamage;
    public DamageZoneType damageZoneType;
    public Vector2 damageZoneArea;
    public float zoneWarningTime;
    public Vector3 attackPosition;
    public float attackYRotation;
    public bool relativePosition;
    public float delayToNextAttack;
}

[CreateAssetMenu(fileName = "Boss Melee Attack", menuName = "Boss Attack/Melee Attack", order = 1)]
public class BossMeleeAttack : BossAttackNode
{
    [SerializeField]
    private MeleeAttack[] attacks;

    public override void PerformAttack(
        BossAttackManager attacker,
        Action OnAttackFinished,
        float damageMultiplier = 1f
    )
    {
        this.OnAttackFinished = OnAttackFinished;

        BossMeleeAttacker meleeAttacker = attacker.GetBossMeleeAttacker();
        meleeAttacker.PerformMeleeAttacks(attacks, damageMultiplier, FinishAttack);
    }

    public void PerformAttack(
        BossAttackManager attacker,
        EventHandler<bool> OnAttackFinished,
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
