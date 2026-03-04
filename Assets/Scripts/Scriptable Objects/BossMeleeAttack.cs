using System;
using System.Collections;
using UnityEngine;

[Serializable]
public struct MeleeAttack
{
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

    [SerializeField]
    private string animationTrigger;

    public override void PerformAttack(Transform attackTransform, Action OnAttackFinished)
    {
        this.OnAttackFinished = OnAttackFinished;

        MeleeAttackManager.Instance.StartAttackPattern(attacks, attackTransform, 1, FinishAttack);
    }

    //Both this and Ranged attack need to have variable damage based on how many times its been failed
    //This melee attack needs to be performed by the BossAttacker to account for hit layer mask, player health, etc
}
