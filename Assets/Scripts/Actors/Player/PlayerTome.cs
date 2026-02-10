using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTome : PlayerWeapon
{
    private bool inputHeld = false;
    private bool chargeTome = false;

    private float chargeAmount = 0f;

    private Coroutine cooldownCoroutine;

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

        cooldownCoroutine = StartCoroutine(ShootCooldown());
    }

    public override void WeaponSpecial() { }

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

        cooldownCoroutine = StartCoroutine(ShootCooldown());
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

        if (cooldownCoroutine != null)
        {
            StopCoroutine(cooldownCoroutine);
        }
    }
}
