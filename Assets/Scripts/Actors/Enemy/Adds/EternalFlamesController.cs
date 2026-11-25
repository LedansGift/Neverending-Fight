using System.Collections;
using UnityEngine;

public class EternalFlamesController : PlayerLockOnEntityController
{
    private float timeBetweenFlameDamages = 1f;

    [SerializeField]
    private MeleeAttack flamesAttack;

    private DamageZone fallDamageZone;

    [SerializeField]
    private GameObject flamesVisual;

    protected override IEnumerator StartEntityAction()
    {
        flamesVisual.SetActive(false);

        yield return base.StartEntityAction();

        fallDamageZone = AttackTelegraphManager.Instance.StartAttack(transform, flamesAttack);

        yield return new WaitForSeconds(flamesAttack.zoneWarningTime);

        AttackHitResolver.HitArea(
            transform,
            flamesAttack,
            DamageZoneType.circle,
            LayerMaskManager.GetAttackLayerMask(),
            attackSignature
        );

        StartCoroutine(FlarePerpetualDamage());
        flamesVisual.SetActive(true);
    }

    private IEnumerator FlarePerpetualDamage()
    {
        yield return new WaitForSeconds(timeBetweenFlameDamages);

        AttackHitResolver.HitArea(
            transform,
            flamesAttack,
            DamageZoneType.circle,
            LayerMaskManager.GetAttackLayerMask(),
            attackSignature
        );

        StartCoroutine(FlarePerpetualDamage());
    }

    protected override void StopEntityAction()
    {
        base.StopEntityAction();

        flamesVisual.SetActive(false);

        if (fallDamageZone)
        {
            fallDamageZone.DeactivateZone();
        }
    }
}
