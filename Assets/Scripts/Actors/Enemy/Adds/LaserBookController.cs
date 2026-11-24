using System.Collections;
using UnityEngine;

public class LaserBookController : ProjectileEntityController
{
    [SerializeField]
    private MeleeAttack laserAttack;

    private DamageZone laserDamageZone;

    protected override IEnumerator StartEntityAction()
    {
        yield return base.StartEntityAction();

        laserDamageZone = AttackTelegraphManager.Instance.StartAttack(transform, laserAttack);

        yield return new WaitForSeconds(laserAttack.zoneWarningTime);

        AttackHitResolver.HitArea(
            transform,
            laserAttack,
            DamageZoneType.box,
            LayerMaskManager.GetAttackLayerMask(),
            "LaserBook"
        );
    }

    protected override void ToggleEntityActive(object sender, bool toggle)
    {
        if (toggle)
        {
            StartCoroutine(StartEntityAction());
        }
        else
        {
            if (laserDamageZone)
            {
                laserDamageZone.DeactivateZone();
            }

            StopAllCoroutines();
        }
    }
}
