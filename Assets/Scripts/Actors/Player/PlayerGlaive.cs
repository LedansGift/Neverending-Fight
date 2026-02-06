using System.Collections.Generic;
using UnityEngine;

public class PlayerGlaive : PlayerWeapon
{
    public override void WeaponAttackStart()
    {
        weaponAnimator.SetTrigger("attack");

        Health[] hitObjects = GetHitObjects();

        foreach (Health health in hitObjects)
        {
            health.TakeDamage(playerStats.GetGlaiveDamage());
        }
    }

    private Health[] GetHitObjects()
    {
        Collider[] colliders = Physics.OverlapSphere(
            transform.position,
            playerStats.GetGlaiveAttackRange(),
            attackLayerMask
        );

        List<Health> healths = new List<Health>();

        Vector3 glaiveDirection = transform.forward;

        foreach (Collider collider in colliders)
        {
            Vector3 colliderDirection = (
                collider.transform.position - transform.position
            ).normalized;

            if (
                Vector3.Dot(glaiveDirection, colliderDirection)
                < (1f - playerStats.GetGlaiveAttackArc())
            )
            {
                continue;
            }

            if (collider.TryGetComponent<Health>(out Health hitHealth) && !hitHealth.GetIsPlayer())
            {
                healths.Add(hitHealth);
            }
        }

        return healths.ToArray();
    }

    public override void WeaponAttackEnd() { }

    public override void WeaponSpecial() { }
}
