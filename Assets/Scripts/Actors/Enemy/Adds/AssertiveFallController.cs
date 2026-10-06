using System.Collections;
using UnityEngine;

public class AssertiveFallController : ProjectileEntityController
{
    [SerializeField]
    private MeleeAttack fallAttack;

    private DamageZone fallDamageZone;

    [SerializeField]
    private ObjectFollower follower;

    protected override IEnumerator StartEntityAction()
    {
        yield return base.StartEntityAction();

        fallDamageZone = AttackTelegraphManager.Instance.StartAttack(transform, fallAttack);
        follower.ToggleFollow(false);

        yield return new WaitForSeconds(fallAttack.zoneWarningTime);

        AttackHitResolver.HitCircleArea(
            transform,
            fallAttack,
            LayerMaskManager.GetAttackLayerMask()
        );
    }

    protected override void ToggleEntityActive(object sender, bool toggle)
    {
        if (toggle)
        {
            follower.SetFollowTarget(PlayerIdentifier.PlayerTransform);
            follower.ToggleFollow(true);

            StartCoroutine(StartEntityAction());
        }
        else
        {
            if (fallDamageZone)
            {
                fallDamageZone.DeactivateZone();
            }
            follower.ToggleFollow(false);

            StopAllCoroutines();
        }
    }
}
