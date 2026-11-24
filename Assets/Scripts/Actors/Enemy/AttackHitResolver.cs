using System.Collections.Generic;
using UnityEngine;

public class AttackHitResolver
{
    private const float RAIDWIDE_DAMAGE_AREA = 100f;

    public static bool HitArea(
        Transform attackTransform,
        MeleeAttack attack,
        DamageZoneType damageZoneType,
        LayerMask attackLayerMask,
        string attackSignature,
        float damageMult = 1f
    )
    {
        Collider[] hitTargets = GetHitTargets(
            attackTransform,
            attack,
            damageZoneType,
            attackLayerMask,
            out bool raidwideHit
        );

        bool targetHit = false;

        foreach (Collider target in hitTargets)
        {
            if (target.TryGetComponent<Health>(out Health hitHealth) && hitHealth.GetIsPlayer())
            {
                if (attack.knockbackStrength > 0f)
                {
                    KnockbackTarget(attackTransform, hitHealth, attack.knockbackStrength);
                }

                if (
                    hitHealth.TakeDamage(
                        Mathf.RoundToInt(attack.attackDamage * damageMult),
                        raidwideHit
                    )
                )
                {
                    PlayerHealth playerHealth = hitHealth as PlayerHealth;
                    playerHealth.SetAttackFailSignature(attackSignature);
                }
                //Debug.Log("Damage dealt: " + Mathf.RoundToInt(attack.attackDamage * damageMult));
                targetHit = true;
            }
        }

        return targetHit;
    }

    private static Collider[] GetHitTargets(
        Transform attackTransform,
        MeleeAttack attack,
        DamageZoneType damageZoneType,
        LayerMask attackLayerMask,
        out bool raidwideHit
    )
    {
        raidwideHit = false;
        switch (damageZoneType)
        {
            case DamageZoneType.circle:
                return HitCircleArea(attackTransform, attack, attackLayerMask);

            case DamageZoneType.box:
                return HitBoxArea(attackTransform, attack, attackLayerMask);

            case DamageZoneType.donut:
                return HitDonutArea(attackTransform, attack, attackLayerMask);

            case DamageZoneType.raidwide:
                raidwideHit = true;
                return HitRaidwideArea(attackTransform, attack, attackLayerMask);

            default:
                return HitBoxArea(attackTransform, attack, attackLayerMask);
        }
    }

    private static Collider[] HitBoxArea(
        Transform attackTransform,
        MeleeAttack attack,
        LayerMask attackLayerMask
    )
    {
        Vector3 zoneSpawnLocation;
        Quaternion zoneSpawnRotation;
        if (attack.relativePosition)
        {
            if (attack.relativePositionToForward)
            {
                Quaternion forwardRotation = Quaternion.FromToRotation(
                    Vector3.forward,
                    attackTransform.transform.forward
                );
                Vector3 newAttackPosition = forwardRotation * attack.attackPosition;

                zoneSpawnLocation = attackTransform.transform.position + newAttackPosition;
            }
            else
            {
                zoneSpawnLocation = attackTransform.position + attack.attackPosition;
            }

            zoneSpawnRotation = Quaternion.Euler(
                0f,
                attackTransform.eulerAngles.y + attack.attackYRotation,
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

        return hitTargets;
    }

    private static Collider[] HitCircleArea(
        Transform attackTransform,
        MeleeAttack attack,
        LayerMask attackLayerMask
    )
    {
        Vector3 zoneSpawnLocation;
        Vector3 zoneSpawnForward;
        if (attack.relativePosition)
        {
            if (attack.relativePositionToForward)
            {
                Quaternion forwardRotation = Quaternion.FromToRotation(
                    Vector3.forward,
                    attackTransform.transform.forward
                );
                Vector3 newAttackPosition = forwardRotation * attack.attackPosition;

                zoneSpawnLocation = attackTransform.transform.position + newAttackPosition;
            }
            else
            {
                zoneSpawnLocation = attackTransform.position + attack.attackPosition;
            }

            zoneSpawnForward = (
                Quaternion.Euler(0f, attack.attackYRotation, 0f) * attackTransform.forward
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

        List<Collider> validTargets = new List<Collider>();

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

            validTargets.Add(target);
        }

        return validTargets.ToArray();
    }

    private static Collider[] HitDonutArea(
        Transform attackTransform,
        MeleeAttack attack,
        LayerMask attackLayerMask
    )
    {
        Vector3 zoneSpawnLocation;

        if (attack.relativePosition)
        {
            if (attack.relativePositionToForward)
            {
                Quaternion forwardRotation = Quaternion.FromToRotation(
                    Vector3.forward,
                    attackTransform.transform.forward
                );
                Vector3 newAttackPosition = forwardRotation * attack.attackPosition;

                zoneSpawnLocation = attackTransform.transform.position + newAttackPosition;
            }
            else
            {
                zoneSpawnLocation = attackTransform.position + attack.attackPosition;
            }
        }
        else
        {
            zoneSpawnLocation = attack.attackPosition;
        }

        float donutHoleSize = attack.damageZoneArea.y;

        Collider[] hitTargets = Physics.OverlapSphere(
            zoneSpawnLocation,
            attack.damageZoneArea.x,
            attackLayerMask
        );

        List<Collider> validTargets = new List<Collider>();

        foreach (Collider target in hitTargets)
        {
            Vector3 targetPosition = new Vector3(
                target.transform.position.x,
                zoneSpawnLocation.y,
                target.transform.position.z
            );
            float colliderDistance = Vector3.Distance(zoneSpawnLocation, targetPosition);

            if (colliderDistance < (donutHoleSize * attack.damageZoneArea.x))
            {
                continue;
            }

            validTargets.Add(target);
        }

        return validTargets.ToArray();
    }

    private static Collider[] HitRaidwideArea(
        Transform attackTransform,
        MeleeAttack attack,
        LayerMask attackLayerMask
    )
    {
        Vector3 zoneSpawnLocation = attackTransform.position;

        Collider[] hitTargets = Physics.OverlapSphere(
            zoneSpawnLocation,
            RAIDWIDE_DAMAGE_AREA,
            attackLayerMask
        );

        return hitTargets;
    }

    private static void KnockbackTarget(
        Transform attackTransform,
        Health hitHealth,
        float knockbackStrength
    )
    {
        Vector3 knockbackDirection = (
            hitHealth.transform.position - attackTransform.position
        ).normalized;
        knockbackDirection.y = 0;

        hitHealth.Knockback(knockbackDirection, knockbackStrength);
    }
}
