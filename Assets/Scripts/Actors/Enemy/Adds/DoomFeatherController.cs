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

    protected override IEnumerator StartEntityAction()
    {
        featherRB.position = new Vector3(
            transform.position.x,
            featherStartDistance,
            transform.position.z
        );

        yield return new WaitForSeconds(damageZoneAppearTime);
        AttackTelegraphManager.Instance.StartAttack(transform, featherAttack);

        yield return new WaitForSeconds(damageTime - damageZoneAppearTime);
        PerformAttack();
    }

    private void PerformAttack()
    {
        AttackHitResolver.HitCircleArea(
            transform,
            featherAttack,
            LayerMaskManager.GetAttackLayerMask()
        );
    }
}
