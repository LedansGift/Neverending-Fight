using System;
using System.Collections;
using UnityEngine;

public class BossMeleeAttacker : MonoBehaviour
{
    private LayerMask attackLayerMask;

    private void Start()
    {
        attackLayerMask = LayerMaskManager.GetAttackLayerMask();
    }

    private void OnEnable()
    {
        RestartManager.OnResetPhase += ResetMeleeAttacker;
    }

    private void OnDisable()
    {
        RestartManager.OnResetPhase -= ResetMeleeAttacker;
    }

    public void PerformMeleeAttacks(
        MeleeAttack[] meleeAttacks,
        float damageMultiplier,
        Action onAttacksFinished,
        string attackSignature = ""
    )
    {
        AttackTelegraphManager.Instance.StartAttackPattern(transform, meleeAttacks);

        Action finalAttackEvent = null;
        bool finalAttack = false;

        for (int i = 0; i < meleeAttacks.Length; i++)
        {
            if (i >= (meleeAttacks.Length - 1))
            {
                finalAttackEvent = onAttacksFinished;
                finalAttack = true;
            }

            StartCoroutine(
                SetupMeleeAttack(
                    meleeAttacks,
                    i,
                    damageMultiplier,
                    finalAttack,
                    attackSignature,
                    finalAttackEvent
                )
            );
        }
    }

    private IEnumerator SetupMeleeAttack(
        MeleeAttack[] attacks,
        int attackIndex,
        float damageMult,
        bool finalAttack,
        string attackSignature,
        Action onAttacksFinished
    )
    {
        MeleeAttack attack = attacks[attackIndex];

        float attackDelayTime = attack.zoneWarningTime;

        for (int i = 0; i < attackIndex; i++)
        {
            attackDelayTime += attacks[i].delayToNextAttack;
        }

        yield return new WaitForSeconds(attackDelayTime);

        AttackHitResolver.HitArea(
            transform,
            attack,
            attack.damageZoneType,
            attackLayerMask,
            attackSignature,
            damageMult
        );

        if (finalAttack)
        {
            yield return new WaitForSeconds(attack.delayToNextAttack);
            FinishAttacks(onAttacksFinished);
        }
    }

    public void PerformAttackUntelegraphed(
        MeleeAttack attack,
        float damageMult,
        string attackSignature
    )
    {
        AttackHitResolver.HitArea(
            transform,
            attack,
            attack.damageZoneType,
            attackLayerMask,
            attackSignature,
            damageMult
        );
    }

    private void FinishAttacks(Action onAttacksFinished)
    {
        if (onAttacksFinished != null)
        {
            onAttacksFinished();
        }
    }

    public void ResetMeleeAttacker()
    {
        StopAllCoroutines();
    }
}
