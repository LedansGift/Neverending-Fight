using System;
using System.Collections;
using UnityEngine;

public class BossMeleeAttacker : MonoBehaviour
{
    private bool currentAttackFail = false;
    private EventHandler<bool> onAttacksFinished;

    [SerializeField]
    private LayerMask attackLayerMask;

    public void PerformMeleeAttacks(
        MeleeAttack[] meleeAttacks,
        float damageMultiplier,
        EventHandler<bool> onAttacksFinished
    )
    {
        AttackTelegraphManager.Instance.StartAttackPattern(transform, meleeAttacks);

        currentAttackFail = false;
        this.onAttacksFinished = onAttacksFinished;

        bool finalAttack = false;

        for (int i = 0; i < meleeAttacks.Length; i++)
        {
            if (i >= (meleeAttacks.Length - 1))
            {
                finalAttack = true;
            }

            StartCoroutine(SetupMeleeAttack(meleeAttacks, i, damageMultiplier, finalAttack));
        }
    }

    private IEnumerator SetupMeleeAttack(
        MeleeAttack[] attacks,
        int attackIndex,
        float damageMult,
        bool finalAttack
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

        if (!currentAttackFail)
        {
            currentAttackFail = attackFail;
        }

        if (finalAttack)
        {
            yield return new WaitForSeconds(attack.delayToNextAttack);
            FinishAttacks();
        }
    }

    private void FinishAttacks()
    {
        if (onAttacksFinished != null)
        {
            onAttacksFinished(this, currentAttackFail);
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
            zoneSpawnForward = Quaternion.Euler(0f, attack.attackYRotation, 0f) * transform.forward;
        }
        else
        {
            zoneSpawnLocation = attack.attackPosition;
            zoneSpawnForward = Quaternion.Euler(0f, attack.attackYRotation, 0f) * Vector3.forward;
        }

        float attackDotArc = Mathf.Lerp(-1f, 1f, 1f - attack.damageZoneArea.y);

        Collider[] hitTargets = Physics.OverlapSphere(
            zoneSpawnLocation,
            attack.damageZoneArea.x,
            attackLayerMask
        );

        bool targetHit = false;

        foreach (Collider target in hitTargets)
        {
            Vector3 colliderDirection = (target.transform.position - transform.position).normalized;

            if (Vector3.Dot(zoneSpawnForward, colliderDirection) < attackDotArc)
            {
                continue;
            }

            if (target.TryGetComponent<Health>(out Health hitHealth) && hitHealth.GetIsPlayer())
            {
                hitHealth.TakeDamage(Mathf.RoundToInt(attack.attackDamage * damageMult));
                targetHit = true;
            }
        }

        return targetHit;
    }
}
