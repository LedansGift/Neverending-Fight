using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTome : PlayerWeapon
{
    private bool inputHeld = false;
    private bool chargeTome = false;
    private bool specialAvailable = true;

    private float chargeAmount = 0f;

    private Coroutine attackCooldownCoroutine;
    private Coroutine specialCoroutine;

    [SerializeField]
    private TomeAbsorber tomeAbsorber;

    [SerializeField]
    private TomeAttackVisual attackVisual;

    public override void WeaponAttackStart()
    {
        inputHeld = true;

        if (isBusy)
        {
            return;
        }

        //weaponAnimator.SetTrigger("attack");

        chargeTome = true;
        canSwap = false;

        chargeAmount = 0f;

        attackVisual.ActivateVisual(
            playerStats.GetTomeMaxRadius(),
            playerStats.GetTomeChargeTime()
        );
    }

    public override void WeaponAttackEnd()
    {
        inputHeld = false;

        if (!chargeTome)
        {
            return;
        }

        //weaponAnimator.SetTrigger("loose");

        chargeTome = false;
        canSwap = true;

        attackVisual.DeactivateVisual();

        StartExplosion();

        attackCooldownCoroutine = StartCoroutine(ShootCooldown());
    }

    public override void WeaponSpecial()
    {
        if (!specialAvailable)
        {
            return;
        }

        if (isBusy)
        {
            return;
        }

        weaponAnimator.SetTrigger("special");

        specialCoroutine = StartCoroutine(SpecialCast());
    }

    private void Update()
    {
        if (chargeTome)
        {
            chargeAmount = Mathf.Min(
                chargeAmount + Time.deltaTime,
                playerStats.GetTomeChargeTime()
            );

            AdjustPlayerMovement(chargeAmount);

            attackVisual.transform.position = mouseTarget.position;
        }
    }

    private IEnumerator SpecialCast()
    {
        canSwap = false;
        isBusy = true;
        tomeAbsorber.ToggleAbsorber(true);

        yield return new WaitForSeconds(playerStats.GetTomeSpecialCastDuration());

        canSwap = true;
        isBusy = false;
        tomeAbsorber.ToggleAbsorber(false);

        StartCoroutine(BuffCoroutine(tomeAbsorber.GetAbsorbedDamage()));

        if (inputHeld)
        {
            WeaponAttackStart();
        }
    }

    private IEnumerator BuffCoroutine(int absorbedDamage)
    {
        specialAvailable = false;

        int damageBuff = Mathf.Min(absorbedDamage, playerStats.GetTomeSpecialMaxBuff());

        playerStats.SetAttackBuff(damageBuff);

        yield return new WaitForSeconds(playerStats.GetTomeSpecialBuffDuration());

        playerStats.SetAttackBuff(0);

        yield return new WaitForSeconds(
            playerStats.GetTomeSpecialCooldown() - playerStats.GetTomeSpecialBuffDuration()
        );
        specialAvailable = true;
    }

    private IEnumerator ShootCooldown()
    {
        isBusy = true;
        yield return new WaitForSeconds(playerStats.GetTomeAttackCooldown());
        isBusy = false;

        if (inputHeld)
        {
            WeaponAttackStart();
        }
    }

    private void AdjustPlayerMovement(float chargeAmount)
    {
        float lerp = chargeAmount / playerStats.GetTomeChargeTime();
        float slowModifier = Mathf.Lerp(1f, playerStats.GetTomeMaxMovementModifier(), lerp);
        playerMovement.SetWeaponModifier(slowModifier);
    }

    private void StartExplosion()
    {
        playerMovement.SetWeaponModifier();

        float chargeLerp = chargeAmount / playerStats.GetTomeChargeTime();

        float explosionRange = Mathf.Lerp(0f, playerStats.GetTomeMaxRadius(), chargeLerp);

        Health[] hitObjects = GetHitObjects(explosionRange);

        int damage = Mathf.RoundToInt(
            Mathf.Lerp(0f, (float)playerStats.GetTomeMaxDamage(), chargeLerp)
        );

        //Debug.Log("Damage dealt: " + damage);

        foreach (Health health in hitObjects)
        {
            health.TakeDamage(damage);
        }
    }

    private Health[] GetHitObjects(float hitRadius)
    {
        Collider[] colliders = Physics.OverlapSphere(
            attackVisual.transform.position,
            hitRadius,
            attackLayerMask
        );

        List<Health> healths = new List<Health>();

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<Health>(out Health hitHealth) && !hitHealth.GetIsPlayer())
            {
                healths.Add(hitHealth);
            }
        }

        return healths.ToArray();
    }

    private void CancelAttack()
    {
        if (!chargeTome)
        {
            return;
        }

        chargeTome = false;
        canSwap = true;

        attackVisual.DeactivateVisual();

        playerMovement.SetWeaponModifier();

        attackCooldownCoroutine = StartCoroutine(ShootCooldown());
    }

    public override void ActivateWeapon()
    {
        base.ActivateWeapon();

        playerMovement.OnDash += CancelAttack;
    }

    public override void StowWeapon()
    {
        base.StowWeapon();

        inputHeld = false;

        playerMovement.OnDash -= CancelAttack;

        if (attackCooldownCoroutine != null)
        {
            StopCoroutine(attackCooldownCoroutine);
        }

        if (specialCoroutine != null)
        {
            StopCoroutine(specialCoroutine);
        }
    }
}
