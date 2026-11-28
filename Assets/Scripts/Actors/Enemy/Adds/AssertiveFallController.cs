using System.Collections;
using UnityEngine;

public class AssertiveFallController : PlayerLockOnEntityController
{
    [SerializeField]
    private MeleeAttack fallAttack;

    private DamageZone fallDamageZone;

    [SerializeField]
    private VFXPlayer attackFX;

    protected override IEnumerator StartEntityAction()
    {
        yield return base.StartEntityAction();

        fallDamageZone = AttackTelegraphManager.Instance.StartAttack(transform, fallAttack);

        yield return new WaitForSeconds(fallAttack.zoneWarningTime);

        attackFX.PlayVFX(transform.position, transform.rotation);

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
