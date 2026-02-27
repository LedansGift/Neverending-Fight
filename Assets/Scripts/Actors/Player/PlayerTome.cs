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

    private const int TOME_WEAPON_INDEX = 2;

    private Coroutine attackCooldownCoroutine;
    private Coroutine specialCoroutine;

    [SerializeField]
    private ParticleSystem explosionFX;

    [SerializeField]
    private TomeAbsorber tomeAbsorber;

    [SerializeField]
    private TomeAttackVisual attackVisual;

    private void Start()
    {
        explosionFX.transform.SetParent(null);
    }

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

        attackVisual.ActivateZone(
            playerStats.GetTomeMaxRadius(),
            0f,
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

        attackVisual.DeactivateZone(0.15f);

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
        tomeAbsorber.ToggleAbsorber(true, playerStats.GetTomeSpecialMaxBuff());

        yield return new WaitForSeconds(playerStats.GetTomeSpecialCastDuration());

        canSwap = true;
        isBusy = false;
        tomeAbsorber.ToggleAbsorber(false);

        StartCoroutine(
            BuffCoroutine(tomeAbsorber.GetAbsorbedDamage(playerStats.GetTomeSpecialBuffDuration()))
        );

        if (inputHeld)
        {
            WeaponAttackStart();
        }
    }

    private IEnumerator BuffCoroutine(int absorbedDamage)
    {
        specialAvailable = false;

        playerStats.SetAttackBuff(absorbedDamage);

        OnWeaponAbilityCharge?.Invoke(
            this,
            new WeaponAbilityCharge(TOME_WEAPON_INDEX, playerStats.GetTomeSpecialCooldown())
        );

        yield return new WaitForSeconds(playerStats.GetTomeSpecialBuffDuration());

        playerStats.SetAttackBuff(0);

        yield return new WaitForSeconds(
            playerStats.GetTomeSpecialCooldown() - playerStats.GetTomeSpecialBuffDuration()
        );
        specialAvailable = true;

        //update special ability UI
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

        explosionFX.transform.position = mouseTarget.position;
        float explosionFXScale = explosionRange * 0.1f;
        explosionFX.transform.localScale = new Vector3(
            explosionFXScale,
            explosionFXScale,
            explosionFXScale
        );
        explosionFX.gameObject.SetActive(false);
        explosionFX.gameObject.SetActive(true);

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

        attackVisual.DeactivateZone(0.15f);

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

    public override void ResetWeapon()
    {
        CancelAttack();

        inputHeld = false;
        chargeTome = false;
        specialAvailable = true;

        playerStats.SetAttackBuff(0);
        tomeAbsorber.ToggleAbsorber(false);

        OnWeaponAbilityCharge?.Invoke(this, new WeaponAbilityCharge(TOME_WEAPON_INDEX, 0.01f));

        StowWeapon();
        StopAllCoroutines();
    }

    public override int GetWeaponIndex()
    {
        return TOME_WEAPON_INDEX;
    }
}
