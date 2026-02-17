using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGlaive : PlayerWeapon
{
    private bool perfectWindow = false;
    private bool earlyFollowup = false;
    private bool followupSlash = false;
    private bool specialReady = false;
    private bool jumping = false;
    private float slashCharge = 0.5f;
    private float slashHitDelay = 0.15f;
    private float specialCharge = 0f;
    private float jumpTimer = 0f;
    private float jumpHeight = 10f;
    private const int GLAIVE_WEAPON_INDEX = 0;

    [SerializeField]
    private Transform playerJumpTransform;

    [SerializeField]
    private AnimationCurve jumpHeightCurve;

    private Coroutine slashCoroutine;

    public static EventHandler<bool> OnGlaiveSpecial;

    private void Start()
    {
        ChargeSpecial(999f);
    }

    public override void WeaponAttackStart()
    {
        if (perfectWindow && !earlyFollowup)
        {
            ChargeSpecial(playerStats.GetGlaiveSpecialPerfectCharge());
            FollowupSlash();
            return;
        }

        if (isBusy)
        {
            earlyFollowup = true;
            return;
        }

        weaponAnimator.SetTrigger("attack");

        ChargeSpecial(slashCharge);

        slashCoroutine = StartCoroutine(GlaiveSlash());
    }

    public override void WeaponAttackEnd() { }

    public override void WeaponSpecial()
    {
        if (!specialReady || !canSwap)
        {
            return;
        }

        if (slashCoroutine != null)
        {
            StopCoroutine(slashCoroutine);
            earlyFollowup = false;
            followupSlash = false;
        }

        weaponAnimator.SetTrigger("special");

        StartCoroutine(SpecialJump());

        specialCharge = 0f;
        specialReady = false;

        OnWeaponAbilityCharge?.Invoke(
            this,
            new WeaponAbilityCharge(GLAIVE_WEAPON_INDEX, specialCharge)
        );
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

    private void Update()
    {
        if (jumping)
        {
            jumpTimer += Time.deltaTime;
            float jumpProgress = jumpTimer / playerStats.GetGlaiveSpecialJumpTime();

            float jumpValue = jumpHeightCurve.Evaluate(jumpProgress) * jumpHeight;

            playerJumpTransform.position = new Vector3(
                playerJumpTransform.position.x,
                jumpValue,
                playerJumpTransform.position.z
            );
        }
    }

    private IEnumerator SpecialJump()
    {
        isBusy = true;
        canSwap = false;
        jumping = true;
        jumpTimer = 0f;

        playerMovement.SetWeaponModifier(playerStats.GetGlaiveSpecialMovementModifier());

        OnGlaiveSpecial?.Invoke(this, true);

        yield return new WaitForSeconds(playerStats.GetGlaiveSpecialJumpTime());

        OnGlaiveSpecial?.Invoke(this, false);

        playerJumpTransform.position = new Vector3(
            playerJumpTransform.position.x,
            0f,
            playerJumpTransform.position.z
        );

        playerMovement.SetWeaponModifier();

        HitEnemies(playerStats.GetGlaiveSpecialDamage(), playerStats.GetGlaiveSpecialDamageRange());

        jumping = false;
        canSwap = true;
        isBusy = false;
    }

    private IEnumerator GlaiveSlash()
    {
        isBusy = true;
        canSwap = false;

        yield return new WaitForSeconds(slashHitDelay);

        HitEnemies(
            playerStats.GetGlaiveDamage(),
            playerStats.GetGlaiveAttackRange(),
            playerStats.GetGlaiveAttackArc()
        );

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

    private void ChargeSpecial(float chargeAmount)
    {
        specialCharge += chargeAmount;

        if (specialCharge >= playerStats.GetGlaiveSpecialChargeThreshold())
        {
            specialCharge = playerStats.GetGlaiveSpecialChargeThreshold();
            specialReady = true;
        }

        OnWeaponAbilityCharge?.Invoke(
            this,
            new WeaponAbilityCharge(
                GLAIVE_WEAPON_INDEX,
                specialCharge / playerStats.GetGlaiveSpecialChargeThreshold()
            )
        );
    }

    private void HitEnemies(int attackDamage, float attackRange, float attackArc = 2f)
    {
        Health[] hitObjects = GetHitObjects(attackRange, attackArc);

        foreach (Health health in hitObjects)
        {
            health.TakeDamage(attackDamage);
        }
    }

    private Health[] GetHitObjects(float attackRange, float attackArc = 2f)
    {
        Collider[] colliders = Physics.OverlapSphere(
            transform.position,
            attackRange,
            attackLayerMask
        );

        List<Health> healths = new List<Health>();

        Vector3 glaiveDirection = transform.forward;

        foreach (Collider collider in colliders)
        {
            Vector3 colliderDirection = (
                collider.transform.position - transform.position
            ).normalized;

            if (Vector3.Dot(glaiveDirection, colliderDirection) < (1f - attackArc))
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

    public override int GetWeaponIndex()
    {
        return GLAIVE_WEAPON_INDEX;
    }
}
