using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMeleeAttacker : MonoBehaviour
{
    private List<bool> currentAttackFails = new List<bool>();

    [SerializeField]
    private LayerMask attackLayerMask;

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
        EventHandler<bool> onAttacksFinished
    )
    {
        AttackTelegraphManager.Instance.StartAttackPattern(transform, meleeAttacks);

        EventHandler<bool> finalAttackEvent = null;
        currentAttackFails.Add(false);
        int attackFailIndex = currentAttackFails.Count - 1;
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
                    attackFailIndex,
                    damageMultiplier,
                    finalAttack,
                    finalAttackEvent
                )
            );
        }
    }

    private IEnumerator SetupMeleeAttack(
        MeleeAttack[] attacks,
        int attackIndex,
        int attackFailsIndex,
        float damageMult,
        bool finalAttack,
        EventHandler<bool> onAttacksFinished
    )
    {
        MeleeAttack attack = attacks[attackIndex];

        float attackDelayTime = attack.zoneWarningTime;

        for (int i = 0; i < attackIndex; i++)
        {
            attackDelayTime += attacks[i].delayToNextAttack;
        }

        yield return new WaitForSeconds(attackDelayTime);

        bool attackFail = false;

        if (attack.damageZoneType == DamageZoneType.box)
        {
            attackFail = HitBoxArea(attack, damageMult);
        }
        else if (attack.damageZoneType == DamageZoneType.circle)
        {
            attackFail = HitCircleArea(attack, damageMult);
        }
        else
        {
            Debug.Log("Unknown Damage Zone");
        }

        if (!currentAttackFails[attackFailsIndex])
        {
            currentAttackFails[attackFailsIndex] = attackFail;
        }

        if (finalAttack)
        {
            yield return new WaitForSeconds(attack.delayToNextAttack);
            FinishAttacks(currentAttackFails[attackFailsIndex], onAttacksFinished);
        }
    }

    private void FinishAttacks(bool attackFail, EventHandler<bool> onAttacksFinished)
    {
        if (onAttacksFinished != null)
        {
            onAttacksFinished(this, attackFail);
        }
    }

    //REWORK THIS TO UTILISE A GENERIC TYPED MANAGER FOR TELEGRAPHS AND ATTACKS, a la a gridsystem being used for a map vs world grid

    private bool HitBoxArea(MeleeAttack attack, float damageMult)
    {
        Vector3 zoneSpawnLocation;
        Quaternion zoneSpawnRotation;
        if (attack.relativePosition)
        {
            zoneSpawnLocation = transform.position + attack.attackPosition;
            zoneSpawnRotation = Quaternion.Euler(
                0f,
                transform.eulerAngles.y + attack.attackYRotation,
                0f
            );
        }
        else
        {
            zoneSpawnLocation = attack.attackPosition;
            zoneSpawnRotation = Quaternion.Euler(0f, attack.attackYRotation, 0f);
        }

        Vector3 damageZone = new Vector3(attack.damageZoneArea.x, 1f, attack.damageZoneArea.y);

        Collider[] hitTargets = Physics.OverlapBox(
            zoneSpawnLocation,
            damageZone,
            zoneSpawnRotation,
            attackLayerMask
        );

        bool targetHit = false;

        foreach (Collider target in hitTargets)
        {
            if (target.TryGetComponent<Health>(out Health hitHealth) && hitHealth.GetIsPlayer())
            {
                hitHealth.TakeDamage(Mathf.RoundToInt(attack.attackDamage * damageMult));
                //Debug.Log("Damage dealt: " + Mathf.RoundToInt(attack.attackDamage * damageMult));
                targetHit = true;
            }
        }

        return targetHit;
    }

    private bool HitCircleArea(MeleeAttack attack, float damageMult)
    {
        Vector3 zoneSpawnLocation;
        Vector3 zoneSpawnForward;
        if (attack.relativePosition)
        {
            zoneSpawnLocation = transform.position + attack.attackPosition;
            zoneSpawnForward = (
                Quaternion.Euler(0f, attack.attackYRotation, 0f) * transform.forward
            ).normalized;
        }
        else
        {
            zoneSpawnLocation = attack.attackPosition;
            zoneSpawnForward = (
                Quaternion.Euler(0f, attack.attackYRotation, 0f) * Vector3.forward
            ).normalized;
        }

        float attackDotArc = 1f - attack.damageZoneArea.y;

        Collider[] hitTargets = Physics.OverlapSphere(
            zoneSpawnLocation,
            attack.damageZoneArea.x,
            attackLayerMask
        );

        bool targetHit = false;

        foreach (Collider target in hitTargets)
        {
            Vector3 targetPosition = new Vector3(
                target.transform.position.x,
                zoneSpawnLocation.y,
                target.transform.position.z
            );
            Vector3 colliderDirection = (targetPosition - zoneSpawnLocation).normalized;

            float dot = Vector3.Dot(zoneSpawnForward, colliderDirection);
            float dotDivision =
                dot
                / Mathf.Clamp(
                    zoneSpawnForward.magnitude * colliderDirection.magnitude,
                    0.0001f,
                    1f
                );
            float dotArc = 1f - Mathf.Acos(dotDivision) / Mathf.PI;

            if (dotArc < attackDotArc)
            {
                // Debug.Log("Not Hit DOT: " + dotArc);
                // Debug.Log("Collider Direction" + colliderDirection);
                // Debug.Log("Zone Forward" + zoneSpawnForward);
                continue;
            }

            if (target.TryGetComponent<Health>(out Health hitHealth) && hitHealth.GetIsPlayer())
            {
                // Debug.Log("Hit DOT: " + dotArc);
                // Debug.Log("Collider Direction" + colliderDirection);

                hitHealth.TakeDamage(Mathf.RoundToInt(attack.attackDamage * damageMult));
                targetHit = true;
            }
        }

        return targetHit;
    }

    public void ResetMeleeAttacker()
    {
        StopAllCoroutines();
        currentAttackFails = new List<bool>();
    }
}
