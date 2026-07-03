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

    public void PerformAttackPart(
        BossAttackManager attacker,
        Action OnAttackFinished,
        float damageMultiplier = 1f
    )
    {
        BossMeleeAttacker meleeAttacker = attacker.GetBossMeleeAttacker();
        meleeAttacker.PerformMeleeAttacks(attacks, damageMultiplier, OnAttackFinished);
    }

    public override void FinishAttack()
    {
        // tell attack manager that a this attack was failed
        OnAttackFailCheck?.Invoke(this, EventArgs.Empty);

        base.FinishAttack();
    }

    public MeleeAttack[] GetAttacks()
    {
        return attacks;
    }
}
