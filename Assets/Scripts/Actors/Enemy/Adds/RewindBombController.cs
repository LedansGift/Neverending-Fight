using System.Collections;
using UnityEngine;

public class RewindBombController : MonoBehaviour
{
    private int activeFragments;

    private Projectile projectile;

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

    private void Awake()
    {
        projectile = GetComponent<Projectile>();
        projectile.OnProjectileActivated += ToggleBomb;

        foreach (RewindBombFragment fragment in bombFragments)
        {
            fragment.OnDeath += DecrementFragmentCount;
        }
    }

    private void OnDisable()
    {
        projectile.OnProjectileActivated -= ToggleBomb;

        foreach (RewindBombFragment fragment in bombFragments)
        {
            fragment.OnDeath -= DecrementFragmentCount;
        }

        StopAllCoroutines();
    }

    private IEnumerator DelayedDamageZoneSpawn()
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

    private void ToggleBomb(object sender, bool toggle)
    {
        if (toggle)
        {
            StartCoroutine(DelayedDamageZoneSpawn());
        }
        else
        {
            StopAllCoroutines();
        }
    }
}
