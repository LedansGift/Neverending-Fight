using System.Collections;
using UnityEngine;

public class AssertiveFallController : PlayerLockOnEntityController
{
    [SerializeField]
    private MeleeAttack fallAttack;

    private DamageZone fallDamageZone;

    protected override IEnumerator StartEntityAction()
    {
        yield return base.StartEntityAction();

        fallDamageZone = AttackTelegraphManager.Instance.StartAttack(transform, fallAttack);

        yield return new WaitForSeconds(fallAttack.zoneWarningTime);

        AttackHitResolver.HitArea(
            transform,
            fallAttack,
            DamageZoneType.circle,
            LayerMaskManager.GetAttackLayerMask(),
            attackSignature
        );
    }

    protected override void StopEntityAction()
    {
        base.StopEntityAction();

        if (fallDamageZone)
        {
            fallDamageZone.DeactivateZone();
        }
    }
}
