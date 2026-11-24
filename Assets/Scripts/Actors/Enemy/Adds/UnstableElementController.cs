using System.Collections;
using UnityEngine;

public class UnstableElementController : ProjectileEntityController
{
    [SerializeField]
    private MeleeAttack bombAttack;

    private DamageZone bombDamageZone;

    protected override IEnumerator StartEntityAction()
    {
        yield return base.StartEntityAction();

        bombDamageZone = AttackTelegraphManager.Instance.StartAttack(transform, bombAttack);

        yield return new WaitForSeconds(bombAttack.zoneWarningTime);

        AttackHitResolver.HitArea(
            transform,
            bombAttack,
            DamageZoneType.circle,
            LayerMaskManager.GetAttackLayerMask(),
            attackSignature
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
            if (bombDamageZone)
            {
                bombDamageZone.DeactivateZone();
            }

            StopAllCoroutines();
        }
    }
}
