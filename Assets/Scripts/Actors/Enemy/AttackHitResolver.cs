using UnityEngine;

public class AttackHitResolver
{
    public static bool HitBoxArea(
        Transform attackTransform,
        MeleeAttack attack,
        LayerMask attackLayerMask,
        float damageMult = 1f
    )
    {
        Vector3 zoneSpawnLocation;
        Quaternion zoneSpawnRotation;
        if (attack.relativePosition)
        {
            zoneSpawnLocation = attackTransform.position + attack.attackPosition;
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

    public static bool HitCircleArea(
        Transform attackTransform,
        MeleeAttack attack,
        LayerMask attackLayerMask,
        float damageMult = 1f
    )
    {
        Vector3 zoneSpawnLocation;
        Vector3 zoneSpawnForward;
        if (attack.relativePosition)
        {
            zoneSpawnLocation = attackTransform.position + attack.attackPosition;
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
                Debug.Log("Damage dealt");
                // Debug.Log("Collider Direction" + colliderDirection);



                hitHealth.TakeDamage(Mathf.RoundToInt(attack.attackDamage * damageMult));
                targetHit = true;
            }
        }

        return targetHit;
    }
}
