using System;
using UnityEngine;

[Serializable]
public struct MeleeAttack
{
    public MeleeAttack(
        int attackDamage,
        DamageZoneType damageZoneType,
        Vector2 damageZoneArea,
        float zoneWarningTime,
        Vector3 attackPosition = new Vector3(),
        float attackYRotation = 0f,
        bool relativePosition = true,
        float delayToNextAttack = 0f,
        bool relativePositionToForward = false
    )
    {
        this.attackDamage = attackDamage;
        this.damageZoneType = damageZoneType;
        this.damageZoneArea = damageZoneArea;
        this.zoneWarningTime = zoneWarningTime;
        this.attackPosition = attackPosition;
        this.attackYRotation = attackYRotation;
        this.relativePosition = relativePosition;
        this.delayToNextAttack = delayToNextAttack;
        this.relativePositionToForward = relativePositionToForward;
    }

    public int attackDamage;
    public DamageZoneType damageZoneType;
    public Vector2 damageZoneArea;
    public float zoneWarningTime;
    public Vector3 attackPosition;
    public float attackYRotation;
    public bool relativePosition;
    public float delayToNextAttack;
    public bool relativePositionToForward;
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
