using System.Collections;
using UnityEngine;

public class RewindBombController : ProjectileEntityController
{
    private int activeFragments;

    [SerializeField]
    private float damageZoneAppearTime = 4f;

    [SerializeField]
    private float bombExplodeTime = 4.995f;

    [SerializeField]
    private Animator bombAnimator;

    [SerializeField]
    private RewindBombFragment[] bombFragments;

    [SerializeField]
    private MeleeAttack bombAttack;

    protected override void Awake()
    {
        base.Awake();

        foreach (RewindBombFragment fragment in bombFragments)
        {
            fragment.OnDeath += DecrementFragmentCount;
        }
    }

    protected override void OnDisable()
    {
        foreach (RewindBombFragment fragment in bombFragments)
        {
            fragment.OnDeath -= DecrementFragmentCount;
        }

        base.OnDisable();
    }

    protected override IEnumerator StartEntityAction()
    {
        activeFragments = bombFragments.Length;

        foreach (RewindBombFragment fragment in bombFragments)
        {
            fragment.HealToFull();
        }

        bombAnimator.SetTrigger("start");

        yield return new WaitForSeconds(damageZoneAppearTime);
        AttackTelegraphManager.Instance.StartAttack(transform, bombAttack);

        yield return new WaitForSeconds(bombExplodeTime - damageZoneAppearTime);
        PerformAttack();
    }

    private void PerformAttack()
    {
        AttackHitResolver.HitRaidwideArea(
            transform,
            bombAttack,
            LayerMaskManager.GetAttackLayerMask()
        );

        projectile.DeactivateProjectile();
    }

    private void DecrementFragmentCount()
    {
        if (!projectile.IsProjectileActive())
        {
            return;
        }

        activeFragments--;

        if (activeFragments <= 0)
        {
            projectile.DeactivateProjectile();
        }
    }
}
