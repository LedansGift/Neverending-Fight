using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGlaive : PlayerWeapon
{
    private bool perfectWindow = false;
    private bool earlyFollowup = false;
    private bool followupSlash = false;
    private float slashHitDelay = 0.15f;

    private Coroutine slashCoroutine;

    public override void WeaponAttackStart()
    {
        if (perfectWindow && !earlyFollowup)
        {
            FollowupSlash();
            return;
        }

        if (isBusy)
        {
            earlyFollowup = true;
            return;
        }

        weaponAnimator.SetTrigger("attack");

        slashCoroutine = StartCoroutine(GlaiveSlash());
    }

    public override void WeaponAttackEnd() { }

    public override void WeaponSpecial()
    {
        weaponAnimator.SetTrigger("special");
    }

    private void FollowupSlash()
    {
        if (slashCoroutine != null)
        {
            StopCoroutine(slashCoroutine);
        }

        earlyFollowup = false;
        perfectWindow = false;

        followupSlash = !followupSlash;

        if (followupSlash)
        {
            weaponAnimator.SetTrigger("follow");
        }
        else
        {
            weaponAnimator.SetTrigger("attack");
        }

        slashCoroutine = StartCoroutine(GlaiveSlash());
    }

    private IEnumerator GlaiveSlash()
    {
        isBusy = true;
        canSwap = false;

        yield return new WaitForSeconds(slashHitDelay);

        HitEnemies();

        yield return new WaitForSeconds(playerStats.GetGlaiveRhythmStart() - slashHitDelay);

        perfectWindow = true;
        canSwap = true;

        yield return new WaitForSeconds(playerStats.GetGlaiveRhythmDuration());

        perfectWindow = false;

        yield return new WaitForSeconds(
            playerStats.GetGlaiveAttackCooldown()
                - (playerStats.GetGlaiveRhythmStart() + playerStats.GetGlaiveRhythmDuration())
        );

        earlyFollowup = false;
        followupSlash = false;
        isBusy = false;
    }

    private void HitEnemies()
    {
        Health[] hitObjects = GetHitObjects();

        foreach (Health health in hitObjects)
        {
            health.TakeDamage(playerStats.GetGlaiveDamage());
        }
    }

    private Health[] GetHitObjects()
    {
        Collider[] colliders = Physics.OverlapSphere(
            transform.position,
            playerStats.GetGlaiveAttackRange(),
            attackLayerMask
        );

        List<Health> healths = new List<Health>();

        Vector3 glaiveDirection = transform.forward;

        foreach (Collider collider in colliders)
        {
            Vector3 colliderDirection = (
                collider.transform.position - transform.position
            ).normalized;

            if (
                Vector3.Dot(glaiveDirection, colliderDirection)
                < (1f - playerStats.GetGlaiveAttackArc())
            )
            {
                continue;
            }

            if (collider.TryGetComponent<Health>(out Health hitHealth) && !hitHealth.GetIsPlayer())
            {
                healths.Add(hitHealth);
            }
        }

        return healths.ToArray();
    }
}
