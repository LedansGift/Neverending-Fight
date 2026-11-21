using System.Collections;
using UnityEngine;

public class DoomFeatherController : ProjectileEntityController
{
    [SerializeField]
    private float damageZoneAppearTime = 5f;

    [SerializeField]
    private float damageTime = 5.995f;
    private float featherStartDistance = 30f;

    [SerializeField]
    private Rigidbody featherRB;

    [SerializeField]
    private MeleeAttack featherAttack;

    [SerializeField]
    private VFXPlayer featherBeaconVFX;

    [SerializeField]
    private VFXPlayer featherExplodeVFX;

    protected override IEnumerator StartEntityAction()
    {
        featherRB.position = new Vector3(
            transform.position.x,
            featherStartDistance,
            transform.position.z
        );

        featherBeaconVFX.PlayVFX(transform);

        yield return new WaitForSeconds(damageZoneAppearTime);
        AttackTelegraphManager.Instance.StartAttack(transform, featherAttack);

        yield return new WaitForSeconds(damageTime - damageZoneAppearTime);
        PerformAttack();
    }

    protected override void StopEntityAction()
    {
        featherBeaconVFX.StopVFX();

        base.StopEntityAction();
    }

    private void PerformAttack()
    {
        featherExplodeVFX.PlayVFX(transform);

        AttackHitResolver.HitCircleArea(
            transform,
            featherAttack,
            LayerMaskManager.GetAttackLayerMask()
        );
    }
}
